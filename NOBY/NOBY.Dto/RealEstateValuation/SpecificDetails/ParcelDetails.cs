namespace NOBY.Dto.RealEstateValuation.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti P (Pozemek)
/// </summary>
public sealed class ParcelDetails
{
    /// <summary>
    /// Číslo parcely
    /// </summary>
    /// <example>1</example>
    public int? ParcelNumber { get; init; }
}