using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.SearchProducts;

internal sealed class SearchProductsRequestValidator
    : AbstractValidator<SearchProductsRequest>
{
    public SearchProductsRequestValidator()
    {
        RuleFor(t => t.Identity)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.IdentityNotFound);
    }
}
