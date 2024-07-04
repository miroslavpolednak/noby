using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

internal sealed class DeveloperSearchRequestValidator
    : AbstractValidator<OfferDeveloperSearchRequest>
{
    public DeveloperSearchRequestValidator()
    {
        RuleFor(t => t.SearchText)
            .NotEmpty();

        RuleFor(t => t.Pagination).SetValidator(new Validators.PaginationRequestValidator());
    }
}
