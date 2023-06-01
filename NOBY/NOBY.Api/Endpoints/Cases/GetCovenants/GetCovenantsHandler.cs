using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed class GetCovenantsHandler
    : IRequestHandler<GetCovenantsRequest, GetCovenantsResponse>
{
    public async Task<GetCovenantsResponse> Handle(GetCovenantsRequest request, CancellationToken cancellationToken)
    {
        var list = await _productService.GetCovenantList(request.CaseId, cancellationToken);
        return new GetCovenantsResponse
        {
        };
    }

    private readonly IProductServiceClient _productService;

    public GetCovenantsHandler(IProductServiceClient productService)
    {
        _productService = productService;
    }
}
