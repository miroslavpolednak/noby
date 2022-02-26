using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class UpdateHouseholdMediatrRequestValidator
    : AbstractValidator<Dto.UpdateHouseholdMediatrRequest>
{
    public UpdateHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.HouseholdId)
            .GreaterThan(0)
            .WithMessage("HouseholdId must be > 0").WithErrorCode("13000");
    }
}