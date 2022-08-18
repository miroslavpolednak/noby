using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateCase;

internal sealed class CreateCaseRequestValidator
    : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("SalesArrangementId");
    }
}
