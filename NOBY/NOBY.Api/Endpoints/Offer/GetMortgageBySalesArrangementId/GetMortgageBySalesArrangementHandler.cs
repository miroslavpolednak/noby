using DomainServices.OfferService.Clients;
using NOBY.Api.Endpoints.Offer.Dto;
using Abstraction = DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed class GetMortgageBySalesArrangementHandler
    : IRequestHandler<GetMortgageBySalesArrangementRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageBySalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // ziskat offerId z SA
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        // kontrola, zda ma SA OfferId
        if (!salesArrangementInstance.OfferId.HasValue)
            throw new NobyValidationException("SalesArrangement is not linked to any Offer");

        var result = await _offerService.GetMortgageOfferDetail(salesArrangementInstance.OfferId.Value, cancellationToken);

        // predelat z DS na FE Dto
        return new GetMortgageResponse
        {
            OfferId = result.OfferId,
            OfferGuaranteeDateTo = result.BasicParameters.GuaranteeDateTo,
            ResourceProcessId = result.ResourceProcessId,
            SimulationInputs = result.SimulationInputs.ToApiResponse(result.BasicParameters),
            SimulationResults = result.SimulationResults.ToApiResponse(result.SimulationInputs, result.AdditionalSimulationResults),
            CreditWorthinessSimpleInputs = result.CreditWorthinessSimpleInputs.ToApiResponse(result.IsCreditWorthinessSimpleRequested),
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