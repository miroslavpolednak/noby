using CIS.Core.Validation;
using FluentValidation;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator()
    {
        RuleFor(t => t.HouseholdId)
            .GreaterThan(0).WithMessage("HouseholdId must be > 0");
    }
}
