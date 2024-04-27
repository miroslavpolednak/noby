using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.SbWebApi.Dto.Refinancing;
using ExternalServices.SbWebApi.V1;
using NOBY.Services.MortgageRefinancing;
using NOBY.Services.WorkflowTask;
using _contract = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal class GenerateExtraPaymentDocumentHandler : IRequestHandler<GenerateExtraPaymentDocumentRequest>
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISbWebApiClient _sbWebApi;
    private readonly MortgageRefinancingDocumentService _refinancingDocumentService;

    public GenerateExtraPaymentDocumentHandler(
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ISbWebApiClient sbWebApi,
        MortgageRefinancingDocumentService refinancingDocumentService)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _sbWebApi = sbWebApi;
        _refinancingDocumentService = refinancingDocumentService;
    }

    public async Task Handle(GenerateExtraPaymentDocumentRequest request, CancellationToken cancellationToken)
    {
        await ValidateHandoverTypeDetail(request, cancellationToken);

        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageExtraPayment, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.CaseId, salesArrangement.OfferId!.Value, cancellationToken);
        
        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(null, offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscounted);

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        await UpdateSaParams(request, salesArrangement, cancellationToken);

        await GenerateCalculationDocuments(request, salesArrangement, offer, cancellationToken);

        await CompleteWorkflowProcess(salesArrangement.CaseId, salesArrangement.ProcessId!.Value, cancellationToken);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, (int)SalesArrangementStates.Finished, cancellationToken);
    }

    private async Task ValidateHandoverTypeDetail(GenerateExtraPaymentDocumentRequest request, CancellationToken cancellationToken)
    {
        var handoverTypeDetail = (await _codebookService.HandoverTypeDetails(cancellationToken)).SingleOrDefault(s => s.Id == request.HandoverTypeDetailId);

        if (handoverTypeDetail is null)
            throw new NobyValidationException(90032);
    }

    private async Task<GetOfferResponse> LoadAndValidateOffer(long caseId, int offerId, CancellationToken cancellationToken)
    {
        var offer = await _offerService.GetOffer(offerId, cancellationToken);

        if (offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate < DateTime.UtcNow.ToLocalTime())
            throw new NobyValidationException(90055);

        var simulationRequest = new SimulateMortgageExtraPaymentRequest
        {
            CaseId = caseId,
            SimulationInputs = new MortgageExtraPaymentSimulationInputs
            {
                ExtraPaymentDate = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate,
                ExtraPaymentAmount = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentAmount,
                ExtraPaymentReasonId = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentReasonId,
                IsExtraPaymentFullyRepaid = offer.MortgageExtraPayment.SimulationInputs.IsExtraPaymentFullyRepaid
            },
            BasicParameters = new MortgageExtraPaymentBasicParameters
            {
                FeeAmountDiscounted = offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscounted
            }
        };

        var simulation = await _offerService.SimulateMortgageExtraPayment(simulationRequest, cancellationToken);

        var oldSimulationData = new OfferSimulationData(offer.MortgageExtraPayment.SimulationResults);
        var newSimulationData = new OfferSimulationData(simulation.SimulationResults);

        if (!oldSimulationData.Equals(newSimulationData))
            throw new NobyValidationException(90055);

        return offer;
    }

    private async Task UpdateSaParams(GenerateExtraPaymentDocumentRequest request, _contract.SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        //Parameters 
        var saRequest = new UpdateSalesArrangementParametersRequest
        {

        };

        await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
    }

    private async Task GenerateCalculationDocuments(GenerateExtraPaymentDocumentRequest request, _contract.SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        await _sbWebApi.GenerateCalculationDocuments(new GenerateCalculationDocumentsRequest
        {
            CaseId = salesArrangement.CaseId,
            IsExtraPaymentComplete = offer.MortgageExtraPayment.SimulationInputs.IsExtraPaymentFullyRepaid,
            ExtraPaymentDate = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate,
            ClientKbId = request.ClientKbId,
            ExtraPaymentAmount = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentAmount,
            FeeAmount = offer.MortgageExtraPayment.SimulationResults.FeeAmount,
            PrincipalAmount = offer.MortgageExtraPayment.SimulationResults.PrincipalAmount,
            InterestAmount = offer.MortgageExtraPayment.SimulationResults.InterestAmount,
            OtherUnpaidFees = offer.MortgageExtraPayment.SimulationResults.OtherUnpaidFees,
            InterestOnLate = offer.MortgageExtraPayment.SimulationResults.InterestOnLate,
            InterestCovid = offer.MortgageExtraPayment.SimulationResults.InterestCovid,
            IsLoanOverdue = offer.MortgageExtraPayment.SimulationResults.IsLoanOverdue,
            IsPaymentReduced = offer.MortgageExtraPayment.SimulationResults.IsPaymentReduced,
            NewMaturityDate = offer.MortgageExtraPayment.SimulationResults.NewMaturityDate,
            NewPaymentAmount = offer.MortgageExtraPayment.SimulationResults.NewPaymentAmount,
        }, cancellationToken);
    }

    private async Task CompleteWorkflowProcess(long caseId, long processId, CancellationToken cancellationToken)
    {
        var process = await _caseService.GetProcessByProcessId(caseId, processId, cancellationToken);

        var completeTaskRequest = new CompleteTaskRequest
        {
            CaseId = caseId,
            TaskIdSb = process.ProcessIdSb
        };

        await _caseService.CompleteTask(completeTaskRequest, cancellationToken);
    }
}