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
}


