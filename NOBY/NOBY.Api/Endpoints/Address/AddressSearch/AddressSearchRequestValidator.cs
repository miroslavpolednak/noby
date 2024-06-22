using FluentValidation;

namespace NOBY.Api.Endpoints.Address.AddressSearch;

internal sealed class AddressSearchRequestValidator
    : AbstractValidator<AddressAddressSearchRequest>
{
    public AddressSearchRequestValidator()
    {
        RuleFor(t => t.SessionId)
            .NotEmpty();

        RuleFor(t => t.SearchText)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(t => t.PageSize)
            .NotEmpty()
            .GreaterThan(0);
    }
}
