using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetListRealEstateValuation;

internal sealed class GetListRealEstateValuationHandler
    : IRequestHandler<GetListRealEstateValuationRequest>
{
    public async Task Handle(GetListRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetListRealEstateValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
