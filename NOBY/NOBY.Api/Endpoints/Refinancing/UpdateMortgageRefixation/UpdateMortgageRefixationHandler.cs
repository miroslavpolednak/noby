using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.MortgageRefinancingWorkflow;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationHandler
    : IRequestHandler<UpdateMortgageRefixationRequest>
{
    public async Task Handle(UpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var workflowResult = await _retentionWorkflowService.GetTaskInfoByTaskId(request.CaseId, salesArrangement.TaskProcessId!.Value, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            TaskProcessId = salesArrangement.TaskProcessId!.Value,
            LoanInterestRateDiscount = request.InterestRateDiscount
        };

        // vytvorit / updatovat IC task pokud je treba
        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(workflowResult.TaskList, mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);

        // ulozit SA params refixace (poznamky)
        await updateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        // presimulovat modelace
        await updateOffers(request, cancellationToken);
    }

    private async Task updateOffers(UpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(request.CaseId, OfferTypes.MortgageRefixation, false, cancellationToken);

        foreach (var offer in offers)
        {
            // mame ulozenou jinou slevu ze sazby nez je v requestu
            if (offer.MortgageRefixation.SimulationInputs.InterestRateDiscount != request.InterestRateDiscount)
            {
                var simulationRequest = new SimulateMortgageRefixationRequest
                {
                    CaseId = request.CaseId,
                    OfferId = offer.Data.OfferId,
                    BasicParameters = offer.MortgageRefixation.BasicParameters,
                    SimulationInputs = offer.MortgageRefixation.SimulationInputs
                };
                simulationRequest.SimulationInputs.InterestRateDiscount = request.InterestRateDiscount;

                // presimulovat
                await _offerService.SimulateMortgageRefixation(simulationRequest, cancellationToken);
            }
        }
    }

    private Task updateSalesArrangementParameters(UpdateMortgageRefixationRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Refixation.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;
        salesArrangement.Refixation.Comment = request.Comment;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Refixation = salesArrangement.Refixation
        }, cancellationToken);
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly MortgageRefinancingWorkflowService _retentionWorkflowService;
    private readonly ICaseServiceClient _caseService;

    public UpdateMortgageRefixationHandler(IOfferServiceClient offerService, ISalesArrangementServiceClient salesArrangementService, MortgageRefinancingWorkflowService retentionWorkflowService, ICaseServiceClient caseService)
    {
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _retentionWorkflowService = retentionWorkflowService;
        _caseService = caseService;
    }
}
