using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationRequestValidator
    : AbstractValidator<PreorderOnlineValuationRequest>
{
    public PreorderOnlineValuationRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.BuildingTechnicalStateCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (t, c) => (await codebookService.RealEstateValuationBuildingTechnicalStates(c)).Any(x => x.Code == t));

        RuleFor(t => t.BuildingMaterialStructureCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (t, c) => (await codebookService.RealEstateValuationBuildingMaterialStructures(c)).Any(x => x.Code == t));

        RuleFor(t => t.FlatSchemaCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (t, c) => (await codebookService.RealEstateValuationFlatSchemas(c)).Any(x => x.Code == t));

        RuleFor(t => t.BuildingAgeCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (t, c) => (await codebookService.RealEstateValuationBuildingAges(c)).Any(x => x.Code == t));

        RuleFor(t => t.FlatArea)
            .NotEmpty();
    }
}
