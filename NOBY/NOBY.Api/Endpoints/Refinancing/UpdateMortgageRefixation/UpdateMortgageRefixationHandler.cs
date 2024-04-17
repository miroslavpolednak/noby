using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.MortgageRefinancing;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationHandler(
    IOfferServiceClient _offerService, 
    ISalesArrangementServiceClient _salesArrangementService, 
    MortgageRefinancingWorkflowService _retentionWorkflowService)
        : IRequestHandler<UpdateMortgageRefixationRequest, UpdateMortgageRefixationResponse>
{
    public async Task<UpdateMortgageRefixationResponse> Handle(UpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            ProcessId = salesArrangement.ProcessId!.Value,
            LoanInterestRateDiscount = request.InterestRateDiscount
        };

        // vytvorit / updatovat IC task pokud je treba
        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);

        // ulozit SA params refixace (poznamky)
        await updateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        // presimulovat modelace
        await updateOffers(request, cancellationToken);

        return new UpdateMortgageRefixationResponse
        {
            ProcessId = salesArrangement.ProcessId!.Value
        };
    }

    private async Task updateOffers(UpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(request.CaseId, OfferTypes.MortgageRefixation, false, cancellationToken);

        foreach (var offer in offers)
        {
            // mame ulozenou jinou slevu ze sazby nez je v requestu
            if (!((OfferFlagTypes)offer.Data.Flags).HasFlag(OfferFlagTypes.LegalNotice)
                && offer.MortgageRefixation.SimulationInputs.InterestRateDiscount != request.InterestRateDiscount)
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
}
