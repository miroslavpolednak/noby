using System.Net;

namespace DomainServices.RiskIntegrationService.Api.Clients;

public static class HttpClientResponseHelper
{
    public static async Task<TResult> CallService<TResult>(
        Func<Task<HttpResponseMessage?>> serviceCall, 
        Func<HttpResponseMessage, Task<TResult>> resultProcesor, 
        string serviceName,
        string endpointName)
    {
        try
        {
            var response = await serviceCall().ConfigureAwait(false);

            if (response!.IsSuccessStatusCode)
            {
                return await resultProcesor(response);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                // asi validacni chyba?
                throw new CisValidationException(0, "validatce");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                throw new CisServiceUnavailableException(serviceName, endpointName, await getRawResponse());
            else if ((int)response.StatusCode >= 500)
                throw new CisServiceServerErrorException(serviceName, endpointName, await getRawResponse());
            else
                throw new CisServiceServerErrorException(serviceName, endpointName, $"{response.StatusCode}: {await getRawResponse()}");
        
            async Task<string> getRawResponse()
                => response.Content == null ? "" : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(serviceName, endpointName, ex.Message);
        }
    }
}
