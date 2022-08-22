using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestProductValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationProduct>
{
    public SaveRequestProductValidator()
    {
        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0);

        RuleFor(t => t.LoanKindId)
            .GreaterThan(0);
    }
}
