using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.GlobalValidators;

internal sealed class IdentityValidator
    : AbstractValidator<Contracts.Shared.Identity?>
{
    public IdentityValidator()
    {
        RuleFor(t => t!.IdentityId)
            .NotEmpty();
        RuleFor(t => t!.IdentityScheme)
            .NotEmpty();
    }
}
