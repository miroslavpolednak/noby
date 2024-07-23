namespace NOBY.Dto.RealEstateValuation;

public class OnlinePreorderData
{
    /// <summary>
    /// Technický stav budovy, hodnota z číselníku <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingTechnicalState">RealEstateValuationBuildingTechnicalState</a>
    /// </summary>
    /// <example>1</example>
    [Required]
    [MinLength(1)]
    public string? BuildingTechnicalStateCode { get; set; } = string.Empty;

    /// <summary>
    /// Konstrukční materiál budovy, hodnota z číselníku <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingMaterialStructure">RealEstateValuationBuildingMaterialStructure</a>
    /// </summary>
    /// <example>CIHLA</example>
    [Required]
    [MinLength(1)]
    public string? BuildingMaterialStructureCode { get; set; } = string.Empty;

    /// <summary>
    /// Dispozice, hodnota z číselníku <a href="https://wiki.kb.cz/display/HT/RealEstateValuationFlatSchema">RealEstateValuationFlatSchema</a>
    /// </summary>
    /// <example>1+1</example>
    [Required]
    [MinLength(1)]
    public string? FlatSchemaCode { get; set; } = string.Empty;

    /// <summary>
    /// Čistá podlahová plocha bytu
    /// </summary>
    /// <example>55</example>
    [Required]
    public decimal? FlatArea { get; set; }

    /// <summary>
    /// Stáří budovy, hodnota z číselníku <a href="https://wiki.kb.cz/display/HT/RealEstateValuationBuildingAge">RealEstateValuationBuildingAge</a>
    /// </summary>
    /// <example>99</example>
    [Required]
    [MinLength(1)]
    public string? BuildingAgeCode { get; set; } = string.Empty;
}
