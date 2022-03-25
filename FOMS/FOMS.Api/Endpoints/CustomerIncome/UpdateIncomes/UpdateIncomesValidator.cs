using FluentValidation;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncomes;

internal class UpdateIncomesValidator
    : AbstractValidator<UpdateIncomesRequest>
{
    public UpdateIncomesValidator()
    {
        RuleForEach(t => t.Incomes)
            .Must(t => t.IncomeTypeId != CIS.Foms.Enums.CustomerIncomeTypes.Unknown || t.IncomeId.GetValueOrDefault() > 0)
            .WithMessage("IncomeTypeId must be > 0");
    }
}
