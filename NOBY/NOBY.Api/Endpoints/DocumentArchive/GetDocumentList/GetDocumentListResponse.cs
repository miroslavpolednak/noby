namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListResponse
{
    public IReadOnlyCollection<Dto.Documents.DocumentsMetadata> DocumentsMetadata { get; set; } = null!;

    public IReadOnlyCollection<Dto.Documents.CategoryEaCodeMain> CategoryEaCodeMain { get; set; } = null!;
}
