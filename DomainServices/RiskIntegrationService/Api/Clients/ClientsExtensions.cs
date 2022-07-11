namespace DomainServices.RiskIntegrationService.Api.Clients;

public static class ClientExtensions
{
    public static async Task<T> ToClientResponse<T>(this HttpResponseMessage response, string url, ILogger logger, CancellationToken cancellationToken) 
    {
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, RiskBusinessCaseStartupExtensions.ServiceName, url, nameof(T));

            logger.ExtServiceResponse(RiskBusinessCaseStartupExtensions.ServiceName, url, response);
            return result;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            // asi validacni chyba?
            throw new CisValidationException(0, "validace");
        }
        else
        {
            // 500?
            throw new CisValidationException(0, "chyba?");
        }
    }
}
