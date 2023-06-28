namespace NOBY.Api.Endpoints.DocumentArchive.SetDocumentStatusInQueue;

public class SetDocumentStatusInQueueRequest : IRequest
{
    public SetDocumentStatusInQueueRequest(string documentId, int statusId)
    {
        DocumentId = documentId;
        StatusId = statusId;
    }

    public string DocumentId { get; }

    public int StatusId { get; }
}
