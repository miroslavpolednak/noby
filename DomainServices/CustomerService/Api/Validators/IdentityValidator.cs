using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

public class IdentityValidator : AbstractValidator<Identity>
{
    public IdentityValidator()
    {
        RuleFor(t => t.IdentityId)
            .GreaterThan(0)
            .WithMessage("IdentityId must be > 0")
            .WithErrorCode("11005");

        RuleFor(t => t.IdentityScheme)
            .IsInEnum()
            .NotEqual(Identity.Types.IdentitySchemes.Unknown)
            .WithMessage("IdentityScheme must be specified")
            .WithErrorCode("11006");
    }
}
