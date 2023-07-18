using System.Text.Json;
using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.RiskIntegrationService.Clients.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.UserService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{

    #region Construction

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ILoanApplicationServiceClient _loanApplicationService;
    private readonly IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IDataAggregatorServiceClient _dataAggregatorService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IUserServiceClient _userService;


    public GetLoanApplicationAssessmentHandler(
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ILoanApplicationServiceClient loanApplicationService,
        IRiskBusinessCaseServiceClient riskBusinessCaseService,
        ICustomerOnSAServiceClient customerOnSAService,
        IDataAggregatorServiceClient dataAggregatorService,
        ICurrentUserAccessor currentUserAccessor,
        IUserServiceClient userService)
    {
        _customerOnSAService = customerOnSAService;
        _dataAggregatorService = dataAggregatorService;
        _currentUserAccessor = currentUserAccessor;
        _userService = userService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
    }

    #endregion

    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // if LoanApplication wasn't created so far, request without NewAssessmentRequired = true is senseless
        if (!request.NewAssessmentRequired && string.IsNullOrWhiteSpace(saInstance.LoanApplicationAssessmentId))
            throw new NobyValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");

        var offer = await _offerService.GetMortgageOfferDetail(saInstance.OfferId!.Value, cancellationToken);

        // create new assesment, if required
        if (request.NewAssessmentRequired)
        {
            if (string.IsNullOrWhiteSpace(saInstance.RiskBusinessCaseId))
                throw new NobyValidationException("SalesArrangement.RiskBusinessCaseId is not defined.");

            await CreateNewAssessment(saInstance, offer, cancellationToken);
        }

        // load assesment by ID
        var assessmentRequest = new RiskBusinessCaseGetAssessmentRequest
        {
            LoanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId,
            RequestedDetails = new List<RiskBusinessCaseRequestedDetails>
            {
                RiskBusinessCaseRequestedDetails.assessmentDetail,
                RiskBusinessCaseRequestedDetails.householdAssessmentDetail,
                RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail,
                RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics
            }
        };

        var assessment = await _riskBusinessCaseService.GetAssessment(assessmentRequest, cancellationToken);

        // convert to ApiResponse
        var response = assessment.ToApiResponse(offer);

        if (response.AssessmentResult == 502 && (response.Reasons?.Any(t => t.Code == "060009") ?? false))
        {
            var customers = await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellationToken);
            foreach (var customer in customers)
            {
                var obligations = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
                if (obligations.Any(t => ((t.Creditor is not null && !t.Creditor.IsExternal.GetValueOrDefault()) && (t.Correction is not null && t.Correction.CorrectionTypeId.GetValueOrDefault() != 1))))
                {
                    response.DisplayAssessmentResultInfoText = true;
                    break;
                }
            }
        }

        return response;
    }

    private async Task CreateNewAssessment(DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, GetMortgageOfferDetailResponse offer, CancellationToken cancellationToken)
    {
        var dataRequest = new GetRiskLoanApplicationDataRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            OfferId = salesArrangement.OfferId!.Value
        };

        var response = await _dataAggregatorService.GetRiskLoanApplicationData(dataRequest, cancellationToken);

        var loanApplicationSaveRequest = JsonSerializer.Deserialize<LoanApplicationSaveRequest>(response.Json)!;

        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        var userIdentity = user.UserIdentifiers.FirstOrDefault();

        loanApplicationSaveRequest.UserIdentity = userIdentity is null ? null : new Identity
        {
            IdentityId = userIdentity.Identity,
            IdentityScheme = userIdentity.IdentityScheme.ToString()
        };

        var riskSegment = await _loanApplicationService.Save(loanApplicationSaveRequest, cancellationToken);

        // create assesment
        var createAssessmentRequest = new RiskBusinessCaseCreateAssessmentRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            RiskBusinessCaseId = salesArrangement.RiskBusinessCaseId,
            // Timestamp, který jsme si uložili pro danou verzi žádosti (dat žádosti), kterou jsme předali v RIP(v2) - POST LoanApplication a tímto danou verzi požadujeme vyhodnotit
            LoanApplicationDataVersion = loanApplicationSaveRequest.LoanApplicationDataVersion,
            AssessmentMode = RiskBusinessCaseAssessmentModes.SC,
            GrantingProcedureCode = offer.SimulationInputs.IsEmployeeBonusRequested == true ? RiskBusinessCaseGrantingProcedureCodes.EMP : RiskBusinessCaseGrantingProcedureCodes.STD,
        };

        var createAssessmentResponse = await _riskBusinessCaseService.CreateAssessment(createAssessmentRequest, cancellationToken);
        salesArrangement.LoanApplicationAssessmentId = createAssessmentResponse.LoanApplicationAssessmentId;

        // update sales arrangement (loanApplicationAssessmentId, riskSegment)
        await _salesArrangementService.UpdateLoanAssessmentParameters(salesArrangement.SalesArrangementId,
                                                                      salesArrangement.LoanApplicationAssessmentId,
                                                                      riskSegment,
                                                                      salesArrangement.CommandId,
                                                                      createAssessmentResponse.RiskBusinessCaseExpirationDate,
                                                                      cancellationToken);
    }
}
