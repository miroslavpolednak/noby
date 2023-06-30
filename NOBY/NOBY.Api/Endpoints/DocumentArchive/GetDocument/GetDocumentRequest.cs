using DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public GetDocumentRequest(string documentId, DocumentSource source)
    {
        DocumentId = documentId;
        Source = source;
    }

    public string DocumentId { get; }
    
    public DocumentSource Source { get; }
}


public enum DocumentSource
{
    EArchive = 0,
    QueueEPodpisy = 1,
    SystemEPodpisy = 2
}