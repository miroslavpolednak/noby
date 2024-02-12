namespace DomainServices.SalesArrangementService.Clients;

public interface IMaintananceService
{
    Task<long[]> GetCancelCaseJobIds(CancellationToken cancellationToken = default);
}
