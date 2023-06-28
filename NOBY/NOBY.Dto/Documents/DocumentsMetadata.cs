namespace NOBY.Dto.Documents;

public class DocumentsMetadata
{
    public string DocumentId { get; set; } = null!;

    public int? EaCodeMainId { get; set; }

    public string FileName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreatedOn { get; set; }

    /// <summary>
    /// Stav přenosu dokumentu do eArchivu. Enum Values: 0 - Uloženo v eArchivu, 1 - Ve zpracování, 2 - Chyba
    /// </summary>
    /// <example>1</example>
    public UploadStatuses UploadStatus { get; set; }
}
