using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler
    : IRequestHandler<PreorderOnlineValuationRequest>
{
    public async Task Handle(PreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {

    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public PreorderOnlineValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
