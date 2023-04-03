using FluentValidation;

namespace NOBY.Api.Endpoints.Address.GetAddressDetail;

internal sealed class GetAddressDetailRequestValidator
    : AbstractValidator<GetAddressDetailRequest>
{
    public GetAddressDetailRequestValidator()
    {
        RuleFor(t => t.SessionId)
            .NotEmpty();

        RuleFor(t => t.AddressId)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(t => t.Title)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(t => t.CountryId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
