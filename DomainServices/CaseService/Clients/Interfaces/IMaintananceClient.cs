namespace DomainServices.CaseService.Clients;

public interface IMaintananceClient
{
    Task<List<long>> GetConfirmedPriceExceptionsRequest(DateTime olderThan, CancellationToken cancellationToken = default);

    Task DeleteConfirmedPriceException(long caseId, CancellationToken cancellationToken = default);
}
