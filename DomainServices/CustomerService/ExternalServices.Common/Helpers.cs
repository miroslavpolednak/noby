using CIS.Core.Exceptions.ExternalServices;
using CIS.Infrastructure.ExternalServicesHelpers;
using System.Net.Http.Json;

namespace DomainServices.CustomerService.ExternalServices.Common;

public static class Helpers
{
    public static async Task<TResult> ProcessResponse<TResult>(string serviceName, HttpResponseMessage? response, CancellationToken cancellationToken)
        where TResult : class
    {
        if (response?.IsSuccessStatusCode ?? false)
        {
            return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, serviceName, nameof(ProcessResponse), nameof(TResult));
        }
        else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadFromJsonAsync<Dto.Error>(cancellationToken: cancellationToken);
            if (error != null)
                throw new CisExtServiceValidationException($"{error.Message}: {error.Detail}");
        }

        throw new CisExtServiceValidationException($"{serviceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }
}
