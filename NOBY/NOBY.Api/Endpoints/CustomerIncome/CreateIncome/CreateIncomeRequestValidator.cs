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
    }
}
