using System.Net;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class HttpClientExtensions
{
    public static async Task<TResult> PostToC4m<TResult>(this HttpClient client, 
        ILogger logger, 
        string serviceName, 
        string endpointName, 
        string url, 
        object request,
        CancellationToken cancellationToken)
        where TResult : class
    {
        try
        {
            logger.ExtServiceRequest(serviceName, endpointName, request);

            var response = await client
                .PostAsJsonAsync(client.BaseAddress + url, request, cancellationToken)
                .ConfigureAwait(false);

            if (response!.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, serviceName, endpointName, nameof(TResult));

                logger.ExtServiceResponse(serviceName, endpointName, result);

                return result;
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
                => response.Content == null ? "" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(serviceName, endpointName, ex.Message);
        }
    }
}
