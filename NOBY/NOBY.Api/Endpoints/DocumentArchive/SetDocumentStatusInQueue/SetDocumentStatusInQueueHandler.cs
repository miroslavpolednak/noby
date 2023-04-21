using DomainServices.DocumentArchiveService.Clients;

namespace NOBY.Api.Endpoints.DocumentArchive.SetDocumentStatusInQueue;

public class SetDocumentStatusInQueueHandler : IRequestHandler<SetDocumentStatusInQueueRequest>
{
    private readonly IDocumentArchiveServiceClient _client;

    public SetDocumentStatusInQueueHandler(IDocumentArchiveServiceClient client)
    {
        _client = client;
    }

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
