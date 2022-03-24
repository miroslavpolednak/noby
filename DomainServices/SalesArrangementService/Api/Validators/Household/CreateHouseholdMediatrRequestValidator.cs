using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class CreateHouseholdMediatrRequestValidator
    : AbstractValidator<Dto.CreateHouseholdMediatrRequest>
{
    public CreateHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");

        RuleFor(t => t.Request.HouseholdTypeId)
            .GreaterThan(0)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");

        RuleFor(t => t.Request.HouseholdTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");
    }
}