namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

/// <summary>
/// Účel úvěru
/// </summary>
public sealed class LoanPurposeItem
{
    /// <summary>
    /// Účel úvěru
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Výše úvěru pro zvolený účel v Kč
    /// </summary>
    public decimal Sum { get; set; }
}

public sealed class LoanRealEstateItemExtended
{
    /// <summary>
    /// Typ nemovitosti
    /// </summary>
    public int RealEstateTypeId { get; set; }

    /// <summary>
    /// Slouží k zajištění
    /// </summary>
    public bool IsCollateral { get; set; }

    /// <summary>
    /// Účel pořízení nemovitosti
    /// </summary>
    public int RealEstatePurchaseTypeId { get; set; }
}

public sealed class CollateralIdentification
{
    /// <summary>
    /// Identifikace nemovitosti
    /// </summary>
    public string? RealEstateIdentification { get; set; }
}
