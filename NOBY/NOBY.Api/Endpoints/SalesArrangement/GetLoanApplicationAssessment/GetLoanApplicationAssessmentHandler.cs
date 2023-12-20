using DomainServices.OfferService.Clients;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.HouseholdService.Contracts;

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

        // customers
        var customers = await _customerOnSAService.GetCustomerList(saInstance.SalesArrangementId, cancellationToken);

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

        foreach (var customer in customers)
        {

            await _customerExposureService.Calculate(new DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2.CustomerExposureCalculateRequest
            {
                LoanApplicationDataVersion = "",
                SalesArrangementId = saInstance.SalesArrangementId,
                RiskBusinessCaseId = saInstance.RiskBusinessCaseId
            }, cancellationToken);
        }

        return await createResult(assessment, offer, customers, cancellationToken);
    }

    private async Task<GetLoanApplicationAssessmentResponse> createResult(
        LoanApplicationAssessmentResponse assessment, 
        GetMortgageOfferResponse offer, 
        List<CustomerOnSA> customers, 
        CancellationToken cancellationToken)
    {
        var response = assessment.ToApiResponse(offer);

        if (response.AssessmentResult == 502 && (response.Reasons?.Any(t => t.Code == "060009") ?? false))
        {
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
        // vytvorit RBC a LA (pokud neexistuji)
        var riskCase = await _riskCaseProcessor.CreateOrUpdateRiskCase(salesArrangement.SalesArrangementId, cancellationToken);

        // create assesment
        var createAssessmentRequest = new RiskBusinessCaseCreateAssessmentRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            RiskBusinessCaseId = riskCase.RiskBusinessCaseId,
            // Timestamp, který jsme si uložili pro danou verzi žádosti (dat žádosti), kterou jsme předali v RIP(v2) - POST LoanApplication a tímto danou verzi požadujeme vyhodnotit
            LoanApplicationDataVersion = riskCase.LoanApplicationDataVersion,
            AssessmentMode = RiskBusinessCaseAssessmentModes.SC,
            GrantingProcedureCode = offer.SimulationInputs.IsEmployeeBonusRequested == true ? RiskBusinessCaseGrantingProcedureCodes.EMP : RiskBusinessCaseGrantingProcedureCodes.STD,
        };

        var createAssessmentResponse = await _riskBusinessCaseService.CreateAssessment(createAssessmentRequest, cancellationToken);

        // update sales arrangement (loanApplicationAssessmentId, riskSegment)
        await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CommandId = salesArrangement.CommandId,
            LoanApplicationAssessmentId = createAssessmentResponse.LoanApplicationAssessmentId,
            RiskBusinessCaseExpirationDate = createAssessmentResponse.RiskBusinessCaseExpirationDate,
            LoanApplicationDataVersion = riskCase.LoanApplicationDataVersion,
            RiskSegment = riskCase.RiskSegment
        }, cancellationToken);
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient _customerExposureService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;

    public GetLoanApplicationAssessmentHandler(
        DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient customerExposureService,
        NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerExposureService = customerExposureService;
        _riskCaseProcessor = riskCaseProcessor;
        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _riskBusinessCaseService = riskBusinessCaseService;
    }
}
