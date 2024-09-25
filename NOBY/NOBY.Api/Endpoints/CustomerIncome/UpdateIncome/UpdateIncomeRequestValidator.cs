using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

internal sealed class UpdateIncomeRequestValidator
    : AbstractValidator<CustomerIncomeUpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator()
    {
        RuleFor(t => t.IncomeTypeId)
            .Must(t => t != EnumIncomeTypes.Unknown);

        RuleFor(t => t.Data.Employment.Job.JobDescription)
            .MaximumLength(50)
            .When(t => t.IncomeTypeId == EnumIncomeTypes.Employement && t.Data?.Employment?.Job is not null);
    }
}
