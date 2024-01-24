namespace NOBY.Dto.RealEstateValuation;

public sealed class RealEstatePriceDetail
{
    /// <summary>
    /// Výsledná cena nemovitosti v Kč
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Název ceny pro Noby
    /// </summary>
    public string PriceTypeName { get; set; } = string.Empty;
}
