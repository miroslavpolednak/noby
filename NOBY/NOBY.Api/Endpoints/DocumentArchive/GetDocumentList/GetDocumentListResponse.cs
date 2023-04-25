using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListResponse
{
    public IReadOnlyCollection<DocumentsMetadata> DocumentsMetadata { get; set; } = null!;
}

public class DocumentsMetadata
{
    public string DocumentId { get; set; } = null!;

    public int? EaCodeMainId { get; set; } 

    public string FileName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreatedOn { get; set; }
    
    /// <summary>
    /// Stav přenosu dokumentu do eArchivu. \n\n<small>Enum Values</small><ul><li>0 - Uloženo v eArchivu</li><li>1 - Ve zpracování</li><li>2 - Chyba</li></ul>
    /// </summary>
    public UploadStatus UploadStatus { get; set; }
}

