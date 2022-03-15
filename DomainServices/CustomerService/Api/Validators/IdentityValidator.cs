using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

public class IdentityValidator : AbstractValidator<CIS.Infrastructure.gRPC.CisTypes.Identity>
{
    public IdentityValidator()
    {
        RuleFor(t => t.IdentityId)
            .GreaterThan(0)
            .WithMessage("IdentityId must be > 0").WithErrorCode("17000");
    }
}
