using DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentMediatrRequest : IRequest<GetDocumentResponse>
{
	public GetDocumentMediatrRequest(string documentId)
	{
        DocumentId = documentId;
    }

    public string DocumentId { get; }
}
