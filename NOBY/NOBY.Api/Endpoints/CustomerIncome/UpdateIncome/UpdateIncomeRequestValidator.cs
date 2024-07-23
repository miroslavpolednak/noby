using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

internal sealed class UpdateIncomeRequestValidator
    : AbstractValidator<CustomerIncomeUpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator()
    {
        RuleFor(t => t.IncomeTypeId)
            .Must(t => t != EnumIncomeTypes.Unknown);
    }
}
