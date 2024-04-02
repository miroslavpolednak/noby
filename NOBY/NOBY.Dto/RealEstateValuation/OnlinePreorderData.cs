namespace NOBY.Dto.RealEstateValuation;

public class OnlinePreorderData
{
    /// <summary>
    /// Technický stav budovy, hodnota z číselníku RealEstateValuationBuildingTechnicalState
    /// </summary>
    /// <example>1</example>
    [Required]
    [MinLength(1)]
    public string BuildingTechnicalStateCode { get; set; } = string.Empty;

    /// <summary>
    /// Konstrukční materiál budovy, hodnota z číselníku RealEstateValuationBuildingMaterialStructure
    /// </summary>
    /// <example>CIHLA</example>
    [Required]
    [MinLength(1)]
    public string BuildingMaterialStructureCode { get; set; } = string.Empty;

    /// <summary>
    /// Dispozice, hodnota z číselníku RealEstateValuationFlatSchema
    /// </summary>
    /// <example>1+1</example>
    [Required]
    [MinLength(1)]
    public string FlatSchemaCode { get; set; } = string.Empty;

    /// <summary>
    /// Čistá podlahová plocha bytu
    /// </summary>
    /// <example>55</example>
    [Required]
    [MinLength(1)]
    public decimal FlatArea { get; set; }

    /// <summary>
    /// Stáří budovy, hodnota z číselníku RealEstateValuationBuildingAge
    /// </summary>
    /// <example>99</example>
    [Required]
    [MinLength(1)]
    public string BuildingAgeCode { get; set; } = string.Empty;
}
