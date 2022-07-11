using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.CreateCase;

internal sealed class CreateCaseRequestValidator
    : AbstractValidator<Contracts.RiskBusinessCase.CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.LoanApplicationIdMp)
            .NotNull()
            .WithErrorCode("0");
    }
}
