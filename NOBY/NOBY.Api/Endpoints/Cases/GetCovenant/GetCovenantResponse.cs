namespace NOBY.Api.Endpoints.Cases.GetCovenant;

public sealed class GetCovenantResponse
{
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

    /// <summary>
    /// Text podmínky (jedná se o text ve smlouvě)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Vysvětlení činnosti, kterou je třeba udělat (vysvětlující text podmínky)
    /// </summary>
    public string? Description { get; set; }
}
