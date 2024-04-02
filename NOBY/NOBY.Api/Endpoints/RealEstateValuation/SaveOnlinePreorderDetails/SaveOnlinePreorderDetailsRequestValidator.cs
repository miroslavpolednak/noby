using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveOnlinePreorderDetails;

internal sealed class SaveOnlinePreorderDetailsRequestValidator
    : AbstractValidator<SaveOnlinePreorderDetailsRequest>
{
    public SaveOnlinePreorderDetailsRequestValidator()
    {
        RuleFor(t => t.BuildingMaterialStructureCode)
            .NotEmpty();

        RuleFor(t => t.BuildingTechnicalStateCode)
            .NotEmpty();

        RuleFor(t => t.FlatSchemaCode)
            .NotEmpty();

        RuleFor(t => t.BuildingAgeCode)
            .NotEmpty();

        RuleFor(t => t.FlatArea)
            .NotEmpty();
    }
}
