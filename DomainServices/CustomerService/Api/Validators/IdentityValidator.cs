using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

public class IdentityValidator : AbstractValidator<CIS.Infrastructure.gRPC.CisTypes.Identity>
{
    public IdentityValidator()
    {
        RuleFor(t => t.IdentityId)
            .GreaterThan(0)
            .WithMessage("IdentityId must be > 0").WithErrorCode("17000");

        RuleFor(t => t.IdentityScheme)
            .Equal(CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
            .WithMessage("IdentityScheme must be 2").WithErrorCode("17000");
    }
}
