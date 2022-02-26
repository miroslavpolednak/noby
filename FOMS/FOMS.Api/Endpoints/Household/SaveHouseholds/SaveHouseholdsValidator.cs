using FluentValidation;

namespace FOMS.Api.Endpoints.Household.SaveHouseholds;

internal class SaveHouseholdsValidator
    : AbstractValidator<SaveHouseholdsRequest>
{
    public SaveHouseholdsValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
        
    }
}