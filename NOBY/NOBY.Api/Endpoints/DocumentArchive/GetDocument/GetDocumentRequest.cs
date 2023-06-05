using DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public GetDocumentRequest(string documentId)
    {
        DocumentId = documentId;
    }

    public string DocumentId { get; }
}


public enum DocumentSource
{
    EArchive = 0,
    QueueEPodpisy = 1,
    SystemEPodpisy = 2
}