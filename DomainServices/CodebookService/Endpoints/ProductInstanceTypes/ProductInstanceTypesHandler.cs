using DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes;

namespace DomainServices.CodebookService.Endpoints.ProductInstanceTypes;

public class ProductInstanceTypesHandler
    : IRequestHandler<ProductInstanceTypesRequest, List<ProductInstanceTypeItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<ProductInstanceTypeItem>> Handle(ProductInstanceTypesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // prilepit jen stavebko

        return new List<ProductInstanceTypeItem>
        {
            new ProductInstanceTypeItem() { Id = 1, Name = "Stavební spoření", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.BuildingSavings },
            new ProductInstanceTypeItem() { Id = 2, Name = "Hypoúvěr", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.BuildingSavingsLoan },
            new ProductInstanceTypeItem() { Id = 3, Name = "Rychloúvěr", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.BuildingSavingsLoan },
            new ProductInstanceTypeItem() { Id = 4, Name = "Rychloúvěr - refix", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.BuildingSavingsLoan },
            new ProductInstanceTypeItem() { Id = 5, Name = "Hypoteční úvěr", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.Mortgage },
            new ProductInstanceTypeItem() { Id = 6, Name = "Americka Hypotéka", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam venenatis libero urna, in vestibulum augue condimentum ultricies.", ProductCategory = ProductInstanceTypeCategory.Mortgage },
        };
    }
}
