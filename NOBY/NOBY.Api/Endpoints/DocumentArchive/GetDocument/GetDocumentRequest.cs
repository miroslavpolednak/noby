namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public GetDocumentRequest(EnumDocumentSource source, string? documentId, string? externalId)
    {
        Source = source;
        ExternalId = externalId;
        DocumentId = documentId;
    }

    public EnumDocumentSource Source { get; }
    
    public string? DocumentId { get; }
    
    public string? ExternalId { get; }
}
