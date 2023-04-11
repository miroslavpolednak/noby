using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.ProfileCheck;

internal abstract class ProfileCheckRequestValidator : AbstractValidator<ProfileCheckRequest>
{
    public ProfileCheckRequestValidator()
    {
        RuleFor(r => r.Identity).SetValidator(new IdentityValidator());

        RuleFor(r => r.Identity.IdentityScheme)
            .Equal(Identity.Types.IdentitySchemes.Kb)
            .WithMessage("Invalid identity scheme.")
            .WithErrorCode("11004");

        RuleFor(r => r.CustomerProfileCode)
            .NotEmpty()
            .WithMessage("CustomerProfileCode must be specified")
            .WithErrorCode("11007");
    }
}