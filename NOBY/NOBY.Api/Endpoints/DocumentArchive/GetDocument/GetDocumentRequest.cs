namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public GetDocumentRequest(Source source, string? documentId, string? externalId)
    {
        Source = source;
        ExternalId = externalId;
        DocumentId = documentId;
    }

    public Source Source { get; }
    
    public string? DocumentId { get; }
    
    public string? ExternalId { get; }
}

public enum Source
{
    EArchive = 0,
    ESignature = 1
}