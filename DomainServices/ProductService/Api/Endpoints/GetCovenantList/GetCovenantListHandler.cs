using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}