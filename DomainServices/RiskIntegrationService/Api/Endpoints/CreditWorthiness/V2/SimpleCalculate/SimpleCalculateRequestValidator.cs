using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

public class SimpleCalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.V2.CreditWorthinessSimpleCalculateRequest>
{
    public SimpleCalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty()
            .WithErrorCode("ResourceProcessId");

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode("Product")
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.ProductTypeId)
                    .GreaterThan(0)
                    .WithErrorCode("Product.ProductTypeId");
                t.RuleFor(t => t!.LoanInterestRate)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanInterestRate");
                t.RuleFor(t => t!.LoanAmount)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanAmount");
                t.RuleFor(t => t!.LoanPaymentAmount)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanPaymentAmount");
                t.RuleFor(t => t!.FixedRatePeriod)
                    .GreaterThan(0)
                    .WithErrorCode("Product.FixedRatePeriod");
                t.RuleFor(t => t!.LoanDuration)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanDuration");
            })
            .WithErrorCode("Product");
    }
}
