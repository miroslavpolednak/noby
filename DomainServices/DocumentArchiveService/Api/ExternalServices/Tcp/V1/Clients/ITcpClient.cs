namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1.Clients;

public interface ITcpClient
{
    public Task<byte[]> DownloadFile(string url, CancellationToken cancellationToken);
}
