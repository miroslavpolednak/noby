using FluentValidation;

namespace NOBY.Api.Endpoints.Address.KatuzSearch;

internal class KatuzSearchRequestValidator : AbstractValidator<KatuzSearchRequest>
{
    public KatuzSearchRequestValidator()
    {
        RuleFor(r => r.PageSize).NotEmpty();

        RuleFor(r => r.SearchText).NotEmpty();
    }
}