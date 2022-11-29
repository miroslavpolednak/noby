namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Mění výchozí HTTP vyjímky na jejich CIS ekvivalenty.
/// </summary>
public sealed class ErrorHandlingHttpHandler
    : DelegatingHandler
{
    private readonly string _serviceName;

    public ErrorHandlingHttpHandler(string serviceName)
    {
        _serviceName = serviceName;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            int statusCode = (int)response.StatusCode;
            if (response!.IsSuccessStatusCode || (statusCode >= 400 && statusCode < 500))
                return response!;
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), await response.SafeReadAsStringAsync(cancellationToken) ?? "");
            else
                throw new CisServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{(int)response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), ex.Message);
        }
    }
}
