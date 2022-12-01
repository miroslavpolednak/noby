using System.Security.Policy;

namespace ExternalServicesTcp.V1.Clients
{
    public interface ITcpClient
    {
        public Task<byte[]> DownloadFile(string url, CancellationToken cancellationToken);
    }
}
