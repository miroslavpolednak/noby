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
    }
}
