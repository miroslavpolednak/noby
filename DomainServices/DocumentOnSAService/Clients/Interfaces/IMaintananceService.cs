using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients;

public interface IMaintananceService
{
    Task<GetUpdateDocumentStatusIdsResponse> GetUpdateDocumentStatusIds(CancellationToken cancellationToken);

    Task<GetCheckDocumentsArchivedIdsResponse> GetCheckDocumentsArchivedIds(int maxBatchSize, CancellationToken cancellationToken);
}
