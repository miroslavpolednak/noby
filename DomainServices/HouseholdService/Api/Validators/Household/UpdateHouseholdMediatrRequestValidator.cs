using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators.Household;

internal class UpdateHouseholdMediatrRequestValidator
    : AbstractValidator<Dto.UpdateHouseholdMediatrRequest>
{
    public UpdateHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.HouseholdId)
            .GreaterThan(0)
            .WithMessage("HouseholdId must be > 0").WithErrorCode("16080");

        RuleFor(t => t.Request.CustomerOnSAId1)
            .NotNull()
            .When(t => t.Request.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}