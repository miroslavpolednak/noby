using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.ProductTypeId)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                t.RuleFor(t => t!.LoanInterestRate)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                t.RuleFor(t => t!.LoanAmount)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                t.RuleFor(t => t!.LoanPaymentAmount)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                t.RuleFor(t => t!.FixedRatePeriod)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                t.RuleFor(t => t!.LoanDuration)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
            });

        RuleFor(t => t.Households)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleForEach(t => t.Households)
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.Customers)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                t.RuleForEach(t => t!.Customers)
                    .ChildRules(x =>
                    {
                        x.When(z => z.Incomes is not null, () =>
                        {
                            x.RuleForEach(t => t!.Incomes)
                                .ChildRules(y =>
                                {
                                    y.RuleFor(t => t!.IncomeTypeId)
                                        .GreaterThan(0);
                                })
                                .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                        });
                    })
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
            })
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.UserIdentity)
            .SetValidator(new IdentityValidator())
            .When(t => t.UserIdentity is not null);
    }
}
