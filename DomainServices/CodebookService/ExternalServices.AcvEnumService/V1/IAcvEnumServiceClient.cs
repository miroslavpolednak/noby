using CIS.Infrastructure.ExternalServicesHelpers;
using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.ExternalServices.AcvEnumService.V1;

public interface IAcvEnumServiceClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<List<Contracts.EnumItemDTO>> GetCategory(Categories category, CancellationToken cancellationToken = default);
}

public enum Categories
{
    [Display(Name = "model_flat_type")]
    ModelFlatType,

    [Display(Name = "model_material_structure")]
    ModelMaterialStructure,

    [Display(Name = "model_age")]
    ModelAge,

    [Display(Name = "model_technical_state")]
    ModelTechnicalState,
}