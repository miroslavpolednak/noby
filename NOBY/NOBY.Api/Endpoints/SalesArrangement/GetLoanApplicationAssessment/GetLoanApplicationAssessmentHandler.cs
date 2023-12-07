﻿using System.Text.Json;
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
using SharedTypes.Enums;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{
    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // if LoanApplication wasn't created so far, request without NewAssessmentRequired = true is senseless
        if (!request.NewAssessmentRequired && string.IsNullOrWhiteSpace(saInstance.LoanApplicationAssessmentId))
        {
            throw new NobyValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");
        }

        // instance of Offer
        var offer = await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken);

        // create new assesment, if required
        if (request.NewAssessmentRequired)
        {
            await createNewAssessment(saInstance, offer, cancellationToken);

            await _salesArrangementService.SetFlowSwitch(saInstance.SalesArrangementId, FlowSwitches.ScoringPerformedAtleastOnce, true, cancellationToken);
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

        return await createResult(assessment, offer, cancellationToken);
    }

    private async Task<GetLoanApplicationAssessmentResponse> createResult(LoanApplicationAssessmentResponse assessment, GetMortgageOfferResponse offer, CancellationToken cancellationToken)
    {
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

    private async Task createNewAssessment(
        DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, 
        GetMortgageOfferResponse offer, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(salesArrangement.RiskBusinessCaseId))
        {
            var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
            var debtor = customers.First(t => t.CustomerRoleId == (int)CustomerRoles.Debtor);

            salesArrangement.RiskBusinessCaseId = await _createRiskBusinessCase.Create(salesArrangement.CaseId, salesArrangement.SalesArrangementId, debtor.CustomerOnSAId, debtor.CustomerIdentifiers, cancellationToken);
        }

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

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ILoanApplicationServiceClient _loanApplicationService;
    private readonly IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IDataAggregatorServiceClient _dataAggregatorService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IUserServiceClient _userService;
    private readonly NOBY.Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService _createRiskBusinessCase;

    public GetLoanApplicationAssessmentHandler(
        NOBY.Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService createRiskBusinessCase,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ILoanApplicationServiceClient loanApplicationService,
        IRiskBusinessCaseServiceClient riskBusinessCaseService,
        ICustomerOnSAServiceClient customerOnSAService,
        IDataAggregatorServiceClient dataAggregatorService,
        ICurrentUserAccessor currentUserAccessor,
        IUserServiceClient userService)
    {
        _createRiskBusinessCase = createRiskBusinessCase;
        _customerOnSAService = customerOnSAService;
        _dataAggregatorService = dataAggregatorService;
        _currentUserAccessor = currentUserAccessor;
        _userService = userService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
    }
}
