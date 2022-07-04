using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Calculate;

public class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.CalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessIdMp)
            .NotEmpty();
        RuleFor(t => t.ItChannel)
            .NotEmpty();
        RuleFor(t => t.LoanApplicationProduct)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new LoanApplicationProductValidator());
        RuleForEach(t => t.Households)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .SetValidator(new LoanApplicationHouseholdValidator());
        RuleFor(t => t.HumanUser)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .SetValidator(new HumanUserValidator());
    }
}

public class LoanApplicationHouseholdValidator
    : AbstractValidator<Contracts.CreditWorthiness.LoanApplicationHousehold?>
{
    public LoanApplicationHouseholdValidator()
    {
        RuleFor(t => t!.ChildrenUnderAnd10)
            .NotNull();
        RuleFor(t => t!.ChildrenOver10)
            .NotNull();
        RuleFor(t => t!.Clients)
            .NotEmpty();
    }
}

public class LoanApplicationProductValidator
    : AbstractValidator<Contracts.CreditWorthiness.LoanApplicationProduct?>
{
    public LoanApplicationProductValidator()
    {
        RuleFor(t => t!.Product)
            .GreaterThan(0);
        RuleFor(t => t!.Maturity)
            .GreaterThan(0);
        RuleFor(t => t!.InterestRate)
            .GreaterThan(0);
        RuleFor(t => t!.AmountRequired)
            .GreaterThan(0);
        RuleFor(t => t!.Annuity)
            .GreaterThan(0);
        RuleFor(t => t!.FixationPeriod)
            .GreaterThan(0);
    }
}