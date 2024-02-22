using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Clients;

public interface IMaintananceService
{
    Task<GetUpdateDocumentStatusIdsResponse> GetUpdateDocumentStatusIds(CancellationToken cancellationToken);
}
