using CIS.Core.Exceptions.ExternalServices;
using System.Net;

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

            if (response!.IsSuccessStatusCode || response!.StatusCode == HttpStatusCode.BadRequest || response!.StatusCode == HttpStatusCode.NotFound)
                return response!;

            // mame ambici rozlisovat jednotlive status kody na ruzne vyjimky?
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new System.Security.Authentication.AuthenticationException($"Authentication to {_serviceName} failed");
                case HttpStatusCode.ServiceUnavailable:
                    throw new CisExtServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), await response.SafeReadAsStringAsync(cancellationToken) ?? "");
                default:
                    throw new CisExtServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{(int)response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
            }    
        }
        catch (HttpRequestException ex)
        {
            throw new CisExtServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), ex.Message);
        }
    }
}
