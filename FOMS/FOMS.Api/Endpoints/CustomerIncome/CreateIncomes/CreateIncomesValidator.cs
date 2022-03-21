using FluentValidation;

namespace FOMS.Api.Endpoints.CustomerIncome.CreateIncomes;

internal class CreateIncomesValidator
    : AbstractValidator<CreateIncomesRequest>
{
    public CreateIncomesValidator()
    {
        RuleForEach(t => t.Incomes)
            .Must(t => t.IncomeTypeId != CIS.Foms.Enums.CustomerIncomeTypes.Unknown || t.IncomeId.GetValueOrDefault() > 0)
            .WithMessage("IncomeTypeId must be > 0");
    }
}
