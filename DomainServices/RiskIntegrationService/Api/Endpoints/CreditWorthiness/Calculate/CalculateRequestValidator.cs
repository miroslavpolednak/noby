using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Calculate;

internal class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.CalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessIdMp)
            .NotEmpty();

        RuleFor(t => t.RiskBusinessCaseIdMp)
            .Must(t => long.TryParse(t, out long x));

        RuleFor(t => t.LoanApplicationProduct)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.Product)
                    .GreaterThan(0);
                t.RuleFor(t => t!.Maturity)
                    .GreaterThan(0);
                t.RuleFor(t => t!.InterestRate)
                    .GreaterThan(0);
                t.RuleFor(t => t!.AmountRequired)
                    .GreaterThan(0);
                t.RuleFor(t => t!.Annuity)
                    .GreaterThan(0);
                t.RuleFor(t => t!.FixationPeriod)
                    .GreaterThan(0);
            });

        RuleFor(t => t.Households)
            .NotEmpty();
        RuleForEach(t => t.Households)
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.Clients)
                    .NotEmpty();
                t.RuleForEach(t => t!.Clients)
                    .ChildRules(x =>
                    {
                        x.RuleFor(t => t!.IdMp)
                            .NotEmpty();
                        x.RuleFor(t => t!.LoanApplicationIncome)
                            .NotEmpty();
                        x.RuleForEach(t => t!.LoanApplicationIncome)
                            .ChildRules(y =>
                            {
                                y.RuleFor(t => t!.CategoryMp)
                                    .GreaterThan(0);
                            });
                    });
            });

        RuleFor(t => t.HumanUser)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .SetValidator(new HumanUserValidator());
    }
}
