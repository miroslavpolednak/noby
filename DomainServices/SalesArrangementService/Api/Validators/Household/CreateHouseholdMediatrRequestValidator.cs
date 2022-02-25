using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class CreateHouseholdMediatrRequestValidator
    : AbstractValidator<Dto.CreateHouseholdMediatrRequest>
{
    public CreateHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
        
        RuleFor(t => t.Request.HouseholdTypeId)
            .GreaterThan(0)
            .WithMessage("Household type must not be empty").WithErrorCode("0");
    }
}