using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.FormatAddress;

internal class FormatAddressRequestValidator : AbstractValidator<FormatAddressRequest>
{
    public FormatAddressRequestValidator()
    {
        RuleFor(r => r.Address.City)
            .NotEmpty()
            .WithMessage("City and CountryId must not be empty")
            .WithErrorCode("11034");

        RuleFor(r => r.Address.CountryId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("City and CountryId must not be empty")
            .WithErrorCode("11034");
    }
}