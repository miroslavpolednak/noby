namespace DomainServices.ProductService.Api.Endpoints.SearchProducts;

internal sealed class SearchProductsHandler(LoanRepository _repository)
    : IRequestHandler<SearchProductsRequest, SearchProductsResponse>
{
    public async Task<SearchProductsResponse> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.SearchProducts(request.Identity, cancellationToken);

        return new SearchProductsResponse
        {
            Products = { result }
        };
	}
}
