using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients.Services;

internal sealed class MaintananceService
    : IMaintananceService
{
    public async Task<GetUpdateDocumentStatusIdsResponse> GetUpdateDocumentStatusIds(CancellationToken cancellationToken)
    {
        return await _client.GetUpdateDocumentStatusIdsAsync(new(), cancellationToken: cancellationToken);
    }

    public async Task<GetCheckDocumentsArchivedIdsResponse> GetCheckDocumentsArchivedIds(int maxBatchSize, CancellationToken cancellationToken)
    {
        return await _client.GetCheckDocumentsArchivedIdsAsync(new() { MaxBatchSize = maxBatchSize }, cancellationToken: cancellationToken);
    }

    public async Task UpdateDocumentsIsArchived(int[] documentOnSaIds, CancellationToken cancellationToken)
    {
        var request = new UpdateDocumentsIsArchivedRequest();
        request.DocumentOnSaIds.AddRange(documentOnSaIds);
        await _client.UpdateDocumentsIsArchivedAsync(request, cancellationToken: cancellationToken);
    }

    private readonly Contracts.MaintananceService.MaintananceServiceClient _client;

    public MaintananceService(Contracts.MaintananceService.MaintananceServiceClient client)
    {
        _client = client;
    }
}
