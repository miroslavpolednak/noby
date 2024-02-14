using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateTask;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.TaskIdSb)
           .GreaterThan(0)
           .WithErrorCode(ErrorCodeMapper.TaskIdSBIsEmpty);

        RuleFor(t => t.Retention)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.RetentionNull);

        When(t => t.Retention is not null, () =>
        {
            RuleFor(t => t.Retention.InterestRateValidFrom)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.InterestRateValidFromEmpty);

            RuleFor(t => t.Retention.LoanInterestRate)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateEmpty);

            RuleFor(t => t.Retention.LoanInterestRateProvided)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateProvidedEmpty);

            RuleFor(t => t.Retention.LoanPaymentAmount)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountEmpty);

            RuleFor(t => t.Retention.LoanPaymentAmountFinal)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountFinalEmpty);

            RuleFor(t => t.Retention.FeeSum)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.FeeSumEmpty);

            RuleFor(t => t.Retention.FeeFinalSum)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.FeeFinalSumEmpty);
        });

    }
}
