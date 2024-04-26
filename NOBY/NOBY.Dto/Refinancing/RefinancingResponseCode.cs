namespace NOBY.Dto.Refinancing;

public sealed class RefinancingResponseCode
{
    /// <summary>
    /// ID instance response kódu
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Typ response k=odu
    /// </summary>
    public int? ResponseCodeTypeId { get; set; }

    public DateTime? DataDateTime { get; set; }

    public string? DataBankCode { get; set; }

    public string? DataString { get; set; }

    /// <summary>
    /// Čas vytvoření response kódu
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Kdo vytvořil response kód
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}
