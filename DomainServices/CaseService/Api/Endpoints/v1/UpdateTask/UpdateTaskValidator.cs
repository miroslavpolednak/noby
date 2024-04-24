using DomainServices.CaseService.Contracts;
using FluentValidation;
using Namotion.Reflection;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateTask;

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

        RuleFor(t => t.MortgageRetention)
            .NotNull()
            .When(t => t.AmendmentsCase == UpdateTaskRequest.AmendmentsOneofCase.MortgageRetention)
            .WithErrorCode(ErrorCodeMapper.RetentionNull);

        RuleFor(t => t.MortgageRefixation)
            .NotNull()
            .When(t => t.AmendmentsCase == UpdateTaskRequest.AmendmentsOneofCase.MortgageRefixation)
            .WithErrorCode(ErrorCodeMapper.RetentionNull);

        When(t => t.MortgageRetention is not null, () =>
        {
            RuleFor(t => t.MortgageRetention.InterestRateValidFrom)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.InterestRateValidFromEmpty);

            RuleFor(t => t.MortgageRetention.LoanInterestRate)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateEmpty);

            RuleFor(t => t.MortgageRetention.LoanInterestRateProvided)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateProvidedEmpty);

            RuleFor(t => t.MortgageRetention.LoanPaymentAmount)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountEmpty);

            RuleFor(t => t.MortgageRetention.LoanPaymentAmountFinal)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountFinalEmpty);

            RuleFor(t => t.MortgageRetention.FeeSum)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.FeeSumEmpty);

            RuleFor(t => t.MortgageRetention.FeeFinalSum)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.FeeFinalSumEmpty);
        });

        When(t => t.MortgageRefixation is not null, () =>
        {
            RuleFor(t => t.MortgageRefixation.LoanInterestRate)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateEmpty);

            RuleFor(t => t.MortgageRefixation.LoanInterestRateProvided)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanInterestRateProvidedEmpty);

            RuleFor(t => t.MortgageRefixation.LoanPaymentAmount)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountEmpty);

            RuleFor(t => t.MortgageRefixation.LoanPaymentAmountFinal)
           .NotNull()
           .WithErrorCode(ErrorCodeMapper.LoanPaymentAmountFinalEmpty);
        });
    }
}
