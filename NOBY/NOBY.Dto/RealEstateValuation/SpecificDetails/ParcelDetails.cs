namespace NOBY.Dto.RealEstateValuation.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti P (Pozemek)
/// </summary>
public sealed class ParcelDetails
{
    public List<ParcelNumber>? ParcelNumbers { get; set; }
}

/// <summary>
/// Číslo parcely
/// </summary>
public sealed class ParcelNumber
{
    /// <summary>
    /// Předčíslí čísla parcely
    /// </summary>
    public int? Prefix { get; set; }

    /// <summary>
    /// Číslo parcely
    /// </summary>
    public int? Number { get; set; }
}