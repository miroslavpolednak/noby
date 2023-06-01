using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        var response = new GetCovenantListResponse();
        response.Covenants.AddRange(Enumerable.Range(0, 1).Select(i => new CovenantListItem()
        {
            Name = ""
            
        }));

        return response;
    }
}