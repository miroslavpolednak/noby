namespace NOBY.Infrastructure.Services.UploadDocumentToArchive;

public struct DocumentMetadata
{
    /// <summary>
    /// GUID dokumentu v dočasném úložišti
    /// </summary>
    public Guid TempFileId { get; set; }

    /// <summary>
    /// Jméno souboru
    /// </summary>
    public string FileName { get; set; }

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