using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty();

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.ProductTypeId)
                    .GreaterThan(0);
                t.RuleFor(t => t!.LoanInterestRate)
                    .GreaterThan(0);
                t.RuleFor(t => t!.LoanAmount)
                    .GreaterThan(0);
                t.RuleFor(t => t!.LoanPaymentAmount)
                    .GreaterThan(0);
                t.RuleFor(t => t!.FixedRatePeriod)
                    .GreaterThan(0);
                t.RuleFor(t => t!.LoanDuration)
                    .GreaterThan(0);
            });

        RuleFor(t => t.Households)
            .NotEmpty();
        RuleForEach(t => t.Households)
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.Customers)
                    .NotEmpty();
                t.RuleForEach(t => t!.Customers)
                    .ChildRules(x =>
                    {
                        x.RuleFor(t => t!.InternalCustomerId)
                            .NotEmpty();
                        x.RuleFor(t => t!.Incomes)
                            .NotEmpty();
                        x.RuleForEach(t => t!.Incomes)
                            .ChildRules(y =>
                            {
                                y.RuleFor(t => t!.IncomeTypeId)
                                    .GreaterThan(0);
                            });
                    });
            });

        RuleFor(t => t.UserIdentity)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .SetValidator(new IdentityValidator());
    }
}
