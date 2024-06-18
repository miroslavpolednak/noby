using DomainServices.DocumentArchiveService.Clients;

namespace DomainServices.DocumentArchiveService.Clients.Services;

public class MaintananceService(Contracts.MaintananceService.MaintananceServiceClient client) : IMaintananceService
{
    private readonly Contracts.MaintananceService.MaintananceServiceClient _client = client;

    public async Task DeleteBinDataFromArchiveQueue(CancellationToken cancellationToken)
     => await _client.DeleteDocumentDataFromArchiveQueueAsync(new(), cancellationToken: cancellationToken);
}
