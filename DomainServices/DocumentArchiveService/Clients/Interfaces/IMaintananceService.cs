namespace DomainServices.DocumentArchiveService.Clients;

public interface IMaintananceService
{
    Task DeleteBinDataFromArchiveQueue( CancellationToken cancellationToken);
}
