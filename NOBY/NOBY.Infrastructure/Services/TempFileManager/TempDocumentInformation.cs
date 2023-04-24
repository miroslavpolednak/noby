namespace NOBY.Infrastructure.Services.TempFileManager;

public sealed class TempDocumentInformation
{
    /// <summary>
    /// GUID dokumentu v dočasném úložišti
    /// </summary>
    public Guid TempGuid { get; set; }

    /// <summary>
    /// Jméno souboru
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Id EA kódu
    /// </summary>
    public int? EaCodeMainId { get; set; }

    /// <summary>
    /// Popis dokumentu
    /// </summary>
    public string? Description { get; set; }

    public string? FormId { get; set; }
}
