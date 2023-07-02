using Newtonsoft.Json;

namespace NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti P (Pozemek)
/// </summary>
public class ParcelDetails  : ISpecificDetails
{
    /// <summary>
    /// Číslo parcely
    /// </summary>
    /// <example>1</example>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int? ParcelNumber { get; init; }
}