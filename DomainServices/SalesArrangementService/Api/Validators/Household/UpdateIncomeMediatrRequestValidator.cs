using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class UpdateIncomeMediatrRequestValidator
    : AbstractValidator<Dto.UpdateIncomeMediatrRequest>
{
    public UpdateIncomeMediatrRequestValidator()
    {
        RuleFor(t => t.Request.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16029");
    }
}