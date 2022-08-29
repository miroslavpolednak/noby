using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateCase;

internal sealed class CreateCaseRequestValidator
    : AbstractValidator<Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode("SalesArrangementId");
    }
}
