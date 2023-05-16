using DomainServices.SalesArrangementService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.RiskIntegrationService.Clients.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{

    #region Construction

    private readonly LoanApplicationDataService _loanApplicationDataService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ILoanApplicationServiceClient _loanApplicationService;
    private readonly IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public GetLoanApplicationAssessmentHandler(
        LoanApplicationDataService loanApplicationDataService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ILoanApplicationServiceClient loanApplicationService,
        IRiskBusinessCaseServiceClient riskBusinessCaseService,
        ICustomerOnSAServiceClient customerOnSAService
        )
    {
        _customerOnSAService = customerOnSAService;
        _loanApplicationDataService = loanApplicationDataService;
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

        // if LoanApplication wasn't created so far, request without NewAssessmentRequired = true is senceless
        if (!request.NewAssessmentRequired && string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
            throw new CisValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");

        var loanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId;

        // create new assesment, if required
        if (request.NewAssessmentRequired)
        {
            if (String.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
            {
                throw new CisValidationException(99999, $"SalesArrangement.RiskBusinessCaseId not defined."); //TODO: error code
            }

            // load data
            var loanApplicationData = await _loanApplicationDataService.LoadData(request.SalesArrangementId, cancellationToken);

            // loan application save
            var loanApplicationSaveRequest = loanApplicationData.ToLoanApplicationSaveRequest();
            var riskSegment = await _loanApplicationService.Save(loanApplicationSaveRequest, cancellationToken);

            // create assesment
            var createAssesmentRequest = loanApplicationData.ToRiskBusinessCaseCreateAssesmentRequest();
            var createAssesmentResponse = await _riskBusinessCaseService.CreateAssessment(createAssesmentRequest, cancellationToken);
            loanApplicationAssessmentId = createAssesmentResponse.LoanApplicationAssessmentId;

            // update sales arrangement (loanApplicationAssessmentId, riskSegment)
            await _salesArrangementService.UpdateLoanAssessmentParameters(request.SalesArrangementId, loanApplicationAssessmentId, riskSegment, saInstance.CommandId, createAssesmentResponse?.RiskBusinessCaseExpirationDate, cancellationToken);
        }

        // load assesment by ID
        var getAssesmentRequest = new  RiskBusinessCaseGetAssessmentRequest
        {
            LoanApplicationAssessmentId = loanApplicationAssessmentId!,
            RequestedDetails = new List<RiskBusinessCaseRequestedDetails> { RiskBusinessCaseRequestedDetails.assessmentDetail, RiskBusinessCaseRequestedDetails.householdAssessmentDetail, RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail, RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics }
        };
        var getAssesmentResponse = await _riskBusinessCaseService.GetAssessment(getAssesmentRequest, cancellationToken);

        var offer = saInstance.OfferId.HasValue ? await _offerService.GetMortgageOfferDetail(saInstance.OfferId.Value, cancellationToken) : null;

        // convert to ApiResponse
        var response = getAssesmentResponse.ToApiResponse(offer);

        if (response.AssessmentResult == 502 && (response.Reasons?.Any(t => t.Code == "060009") ?? false))
        {
            var customers = await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellationToken);
            foreach (var customer in customers)
            {
                var obligations = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
                if (obligations.Any(t => ((t.Creditor is not null && t.Creditor.IsExternal.GetValueOrDefault()) && (t.Correction is not null && t.Correction.CorrectionTypeId.GetValueOrDefault() != 1))))
                {
                    response.DisplayAssessmentResultInfoText = true;
                    break;
                }
            }
        }

        return response;
    }
}
