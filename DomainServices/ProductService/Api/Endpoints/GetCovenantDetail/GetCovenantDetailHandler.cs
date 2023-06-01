using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    public Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}