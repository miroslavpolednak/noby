namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1.Clients;

public class TcpClientMock : ITcpClient
{
    private readonly HttpClient _httpClient;

    public TcpClientMock(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<byte[]> DownloadFile(string url, CancellationToken cancellationToken)
    {
        return Task.FromResult(Convert.FromBase64String("VGhpcyBpcyBhIHRlc3Q="));
    }
}
