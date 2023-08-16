using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.GlobalValidators;

internal sealed class IdentityValidator
    : AbstractValidator<Contracts.Shared.Identity?>
{
    public IdentityValidator()
    {
        RuleFor(t => t!.IdentityId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t!.IdentityScheme)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
    }
}
