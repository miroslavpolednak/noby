using FluentValidation;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

internal class UpdateIncomeRequestValidator
    : AbstractValidator<UpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator()
    {
        RuleFor(t => t.IncomeTypeId)
            .Must(t => t != CIS.Foms.Enums.CustomerIncomeTypes.Unknown);
    }
}
