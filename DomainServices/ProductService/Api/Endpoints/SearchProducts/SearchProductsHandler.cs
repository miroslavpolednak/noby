namespace DomainServices.ProductService.Api.Endpoints.SearchProducts;

internal sealed class SearchProductsHandler
    : IRequestHandler<SearchProductsRequest, SearchProductsResponse>
{
    public async Task<SearchProductsResponse> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.SearchProducts(request.Identity, cancellationToken);

        SearchProductsResponse response = new();
        response.Products.AddRange(result);
        return response;
    }

    private readonly LoanRepository _repository;

    public SearchProductsHandler(LoanRepository repository)
    {
        _repository = repository;
    }
}
