using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients.Services;

internal sealed class MaintananceService
    : IMaintananceService
{
    public async Task<GetUpdateDocumentStatusIdsResponse> GetUpdateDocumentStatusIds(CancellationToken cancellationToken)
    {
        return await _client.GetUpdateDocumentStatusIdsAsync(new(), cancellationToken: cancellationToken);
    }

    private readonly Contracts.MaintananceService.MaintananceServiceClient _client;

    public MaintananceService(Contracts.MaintananceService.MaintananceServiceClient client)
    {
        _client = client;
    }
}
