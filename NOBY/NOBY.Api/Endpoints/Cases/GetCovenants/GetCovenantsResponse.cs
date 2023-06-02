namespace NOBY.Api.Endpoints.Cases.GetCovenants;

/// <summary>
/// Seznam podmínek ke splnění
/// </summary>
public sealed class GetCovenantsResponse
{
    /// <summary>
    /// Seznam sekcí
    /// </summary>
    public List<GetCovenantsResponseSection>? Sections { get; set; }
}

/// <summary>
/// Sekce
/// </summary>
public sealed class GetCovenantsResponseSection
{
    /// <summary>
    /// Sekce - číselník CovenantType
    /// </summary>
    public int CovenantTypeId { get; set; }

    /// <summary>
    /// Seznam fází
    /// </summary>
    public List<GetCovenantsResponsePhase>? Phases { get; set; }
}

/// <summary>
/// Fáze
/// </summary>
public sealed class GetCovenantsResponsePhase
{
    /// <summary>
    /// Písmenná identifikace fáze
    /// </summary>
    public string? OrderLetter { get; set; }

    /// <summary>
    /// Název skupiny podmínek (fáze)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Podmínky ke splnění
    /// </summary>
    public List<GetCovenantsResponseCovenant>? Covenants { get; set; }
}

/// <summary>
/// Podmínka ke splnění
/// </summary>
public sealed class GetCovenantsResponseCovenant
{
    /// <summary>
    /// Písmenná identifikace podmínky v sekci/skupině
    /// </summary>
    public string? OrderLetter { get; set; }

    /// <summary>
    /// Jméno podmínky
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Podmínka splněna
    /// </summary>
    public bool IsFulfilled { get; set; }

    /// <summary>
    /// Termín pro splnění podmínky (datum, kdy by mělo byt splněno)
    /// </summary>
    public DateTime? FulfillDate { get; set; }
}