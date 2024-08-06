using DomainServices.DocumentArchiveService.Clients;

namespace NOBY.Api.Endpoints.DocumentArchive.SetDocumentStatusInQueue;

public class SetDocumentStatusInQueueHandler(IDocumentArchiveServiceClient _client) 
    : IRequestHandler<SetDocumentStatusInQueueRequest>
{
    public async Task Handle(SetDocumentStatusInQueueRequest request, CancellationToken cancellationToken)
    {
        await _client.SetDocumentStatusInQueue(new()
        {
            EArchivId = request.DocumentId,
            StatusInQueue = request.StatusId
        },
        cancellationToken);
    }
}
