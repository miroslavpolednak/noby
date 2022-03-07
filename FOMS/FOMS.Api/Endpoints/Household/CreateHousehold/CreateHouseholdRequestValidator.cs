using FluentValidation;

namespace FOMS.Api.Endpoints.Household.CreateHousehold;

internal class CreateHouseholdRequestValidator
    : AbstractValidator<CreateHouseholdRequest>
{
    public CreateHouseholdRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");

        RuleFor(t => t.HouseholdTypeId)
            .GreaterThan(0)
            .WithMessage("HouseholdTypeId must be > 0");
    }
}
