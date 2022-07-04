using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.GlobalValidators;

internal class HumanUserValidator
    : AbstractValidator<Contracts.HumanUser?>
{
    public HumanUserValidator()
    {
        RuleFor(t => t!.Identity)
            .NotEmpty();
        RuleFor(t => t!.IdentityScheme)
            .NotEmpty();
    }
}
