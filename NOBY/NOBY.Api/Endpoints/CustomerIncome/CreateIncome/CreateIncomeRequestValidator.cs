using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

internal sealed class CreateIncomeRequestValidator
    : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        RuleFor(t => t.IncomeTypeId)
            .Must(t => t != CIS.Foms.Enums.CustomerIncomeTypes.Unknown);

        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0);
    }
}
