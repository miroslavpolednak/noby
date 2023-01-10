using CIS.Core.Exceptions.ExternalServices;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public sealed class BadRequestHttpHandler
    : DelegatingHandler
{
    private readonly string _serviceName;

    public BadRequestHttpHandler(string serviceName)
    {
        _serviceName = serviceName;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response!.StatusCode == HttpStatusCode.BadRequest)
        {
            // chyba spravne reportovana z c4m - bude to nekdy takto vypadat?
            Dto.ErrorModel? result = null;
            try
            {
                result = await response.Content.ReadFromJsonAsync<Dto.ErrorModel>(cancellationToken: cancellationToken);
            }
            catch { }

            if (result is null) // nepodarilo se deserializovat na korektni response type
            {
                var message = await response.SafeReadAsStringAsync(cancellationToken);
                throw new CisExtServiceValidationException($"{_serviceName} unknown error 400: {message}");
            }
            else
            {
                throw new CisExtServiceValidationException(result.Code ?? "", result.Message ?? "");
            }
        }

        return response;
    }
}
