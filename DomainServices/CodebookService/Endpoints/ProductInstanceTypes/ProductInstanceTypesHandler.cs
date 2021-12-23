using DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes;

namespace DomainServices.CodebookService.Endpoints.ProductInstanceTypes;

public class ProductInstanceTypesHandler
    : IRequestHandler<ProductInstanceTypesRequest, List<ProductInstanceTypeItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<ProductInstanceTypeItem>> Handle(ProductInstanceTypesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<ProductInstanceTypeItem>
        {
            new ProductInstanceTypeItem() { Id = 1, Name = "Stavební spoření", ProductCategory = ProductInstanceTypeCategory.BuildingSavings },
            new ProductInstanceTypeItem() { Id = 2, Name = "Překlenovací úvěr", ProductCategory = ProductInstanceTypeCategory.BuildingSavingsLoan },
            new ProductInstanceTypeItem() { Id = 3, Name = "Americka Hypotéka", ProductCategory = ProductInstanceTypeCategory.Mortgage },
        };
    }
}
