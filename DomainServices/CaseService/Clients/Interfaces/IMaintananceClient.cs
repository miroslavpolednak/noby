using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients;

public interface IMaintananceClient
{
    Task<List<ConfirmedPriceException>> GetConfirmedPriceExceptionsRequest(DateTime olderThan, CancellationToken cancellationToken = default);

    Task DeleteConfirmedPriceException(long caseId, CancellationToken cancellationToken = default);
}
