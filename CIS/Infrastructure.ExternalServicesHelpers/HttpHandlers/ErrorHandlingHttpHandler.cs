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
    private readonly bool _registerBadRequestAsError;

    public ErrorHandlingHttpHandler(string serviceName, bool registerBadRequestAsError)
    {
        _registerBadRequestAsError = registerBadRequestAsError;
        _serviceName = serviceName;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response;

        try
        {
            response = await base.SendAsync(request, cancellationToken);

            // vyjimka kvuli SB
            if (response!.StatusCode == HttpStatusCode.BadRequest && _registerBadRequestAsError)
            {
                throw new CisExternalServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{(int)response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
            }

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                return response;
            }
        }
        catch (HttpRequestException ex)
        {
            throw new CisExternalServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), ex.Message);
        }

        // mame ambici rozlisovat jednotlive status kody na ruzne vyjimky?
        switch (response?.StatusCode ?? HttpStatusCode.InternalServerError)
        {
            case HttpStatusCode.Unauthorized:
                throw new System.Security.Authentication.AuthenticationException($"Authentication to {_serviceName} failed");
            case HttpStatusCode.ServiceUnavailable:
                throw new CisExternalServiceUnavailableException(_serviceName, request.RequestUri!.ToString(), await response.SafeReadAsStringAsync(cancellationToken) ?? "");
            default:
                throw new CisExternalServiceServerErrorException(_serviceName, request.RequestUri!.ToString(), $"{(int)response!.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
    }
}
