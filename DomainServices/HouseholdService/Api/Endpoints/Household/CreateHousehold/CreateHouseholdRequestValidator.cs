using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdRequestValidator
    : AbstractValidator<Contracts.CreateHouseholdRequest>
{
    public CreateHouseholdRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");

        RuleFor(t => t.HouseholdTypeId)
            .GreaterThan(0)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");

        RuleFor(t => t.HouseholdTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}