using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.Validators;

internal sealed class CreateCaseRequestValidator
    : AbstractValidator<Contracts.RiskBusinessCase.CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.ResourceProcessIdMp)
            .NotEmpty()
            .WithErrorCode("0");
    }
}
