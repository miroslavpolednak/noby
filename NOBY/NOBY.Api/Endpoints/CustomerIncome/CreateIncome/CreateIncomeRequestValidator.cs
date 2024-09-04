using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

internal sealed class CreateIncomeRequestValidator
    : AbstractValidator<CustomerIncomeCreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        RuleFor(t => t.IncomeTypeId)
            .Must(t => t != EnumIncomeTypes.Unknown);

        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0);

        RuleFor(t => t.Data.Employment.Job.JobDescription)
            .MaximumLength(50)
            .When(t => t.IncomeTypeId == EnumIncomeTypes.Employement && t.Data?.Employment?.Job is not null);
    }
}
