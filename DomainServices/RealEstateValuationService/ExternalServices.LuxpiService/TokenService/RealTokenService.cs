using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.TokenService;

internal sealed class RealTokenService
    : ITokenService
{
    public async Task<string> GetToken(string apiKey, CancellationToken cancellationToken)
    {
        var content = new StringContent("\"" + apiKey + "\"", new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json"));
        var response = await _httpClient
            .PostAsync(new Uri(_httpClient.BaseAddress!, "api/Authetication/request-by-apikey"), content, cancellationToken)
            .ConfigureAwait(false);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
#pragma warning disable CS8603 // Possible null reference return.
        return await response!.SafeReadAsStringAsync(cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
    }

    private readonly HttpClient _httpClient;

    public RealTokenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
