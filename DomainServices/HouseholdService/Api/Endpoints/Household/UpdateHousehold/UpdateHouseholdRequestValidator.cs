using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator()
    {
        RuleFor(t => t.HouseholdId)
            .GreaterThan(0)
            .WithMessage("HouseholdId must be > 0").WithErrorCode("16080");

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}