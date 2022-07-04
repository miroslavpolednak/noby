using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Validators;

public class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.CalculateRequest>
{
    public CalculateRequestValidator()
    {

    }
}
