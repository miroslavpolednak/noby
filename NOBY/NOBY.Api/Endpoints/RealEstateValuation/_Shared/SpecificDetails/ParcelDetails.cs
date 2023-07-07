namespace NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti P (Pozemek)
/// </summary>
public class ParcelDetails
{
    /// <summary>
    /// Číslo parcely
    /// </summary>
    /// <example>1</example>
    public int? ParcelNumber { get; init; }
}