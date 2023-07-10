namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public GetDocumentRequest(string documentId)
    {
        DocumentId = documentId;
    }

    public string DocumentId { get; }
}