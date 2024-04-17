using DomainServices.OfferService.Clients.v1;
using NOBY.Api.Endpoints.Offer.SharedDto;
using Abstraction = DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed class GetMortgageBySalesArrangementHandler(
    IMediator _mediator,
    Abstraction.ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService)
        : IRequestHandler<GetMortgageBySalesArrangementRequest, SharedDto.GetMortgageResponse>
{
    public async Task<SharedDto.GetMortgageResponse> Handle(GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // ziskat offerId z SA
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new NobyValidationException("SalesArrangement is not linked to any Offer");

        var offer = await _offerService.GetMortgageDetail(salesArrangementInstance.OfferId.Value, cancellationToken);

        // predelat z DS na FE Dto
        return new GetMortgageResponse
        {
            OfferId = offer.Data.OfferId,
            OfferGuaranteeDateTo = offer.BasicParameters.GuaranteeDateTo,
            ResourceProcessId = offer.Data.ResourceProcessId,
            SimulationInputs = offer.SimulationInputs.ToApiResponse(offer.BasicParameters),
            SimulationResults = offer.SimulationResults.ToApiResponse(offer.SimulationInputs, offer.AdditionalSimulationResults),
            CreditWorthinessSimpleInputs = offer.CreditWorthinessSimpleInputs.ToApiResponse(offer.Data.IsCreditWorthinessSimpleRequested)
        };
    }
}