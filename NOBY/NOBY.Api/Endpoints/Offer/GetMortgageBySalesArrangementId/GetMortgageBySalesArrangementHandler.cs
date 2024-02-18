using DomainServices.OfferService.Clients;
using NOBY.Api.Endpoints.Offer.SharedDto;
using Abstraction = DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed class GetMortgageBySalesArrangementHandler
    : IRequestHandler<GetMortgageBySalesArrangementRequest, SharedDto.GetMortgageResponse>
{
    public async Task<SharedDto.GetMortgageResponse> Handle(GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // ziskat offerId z SA
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new NobyValidationException("SalesArrangement is not linked to any Offer");

        var result = await _offerService.GetOfferDetail(salesArrangementInstance.OfferId.Value, cancellationToken);

        // predelat z DS na FE Dto
        return new GetMortgageResponse
        {
            OfferId = result.Data.OfferId,
            OfferGuaranteeDateTo = result.MortgageOffer.BasicParameters.GuaranteeDateTo,
            ResourceProcessId = result.Data.ResourceProcessId,
            SimulationInputs = result.MortgageOffer.SimulationInputs.ToApiResponse(result.MortgageOffer.BasicParameters),
            SimulationResults = result.MortgageOffer.SimulationResults.ToApiResponse(result.MortgageOffer.SimulationInputs, result.MortgageOffer.AdditionalSimulationResults),
            CreditWorthinessSimpleInputs = result.MortgageOffer.CreditWorthinessSimpleInputs.ToApiResponse(result.MortgageOffer.IsCreditWorthinessSimpleRequested),
        };
    }

    private readonly Abstraction.ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly IMediator _mediator;
    
    public GetMortgageBySalesArrangementHandler(
        IMediator mediator,
        Abstraction.ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _mediator = mediator;
    }
}