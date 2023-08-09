using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationHandler
    : IRequestHandler<OrderRealEstateValuationRequest>
{
    public async Task Handle(OrderRealEstateValuationRequest request, CancellationToken cancellationToken)
    {

    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public OrderRealEstateValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
