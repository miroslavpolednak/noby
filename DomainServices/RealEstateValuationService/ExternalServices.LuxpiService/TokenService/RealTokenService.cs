﻿using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.TokenService;

internal sealed class RealTokenService
    : ITokenService
{
    public async Task<string> GetToken(string apiKey, CancellationToken cancellationToken)
    {
        var content = new StringContent("\"" + apiKey + "\"", new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json"));
        var response = await _httpClient
            .PostAsync(_url, content, cancellationToken)
            .ConfigureAwait(false);
        
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
#pragma warning disable CS8603 // Possible null reference return.
        return await response!.SafeReadAsStringAsync(cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
    }

    private readonly HttpClient _httpClient;
    // neni thread safe, tady je to jedno
    private static string? _url;

    public RealTokenService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        if (string.IsNullOrEmpty(_url))
        {
            _url = _httpClient.BaseAddress!.AbsoluteUri + (_httpClient.BaseAddress!.AbsoluteUri[^1] == '/' ? "" : "/") + "api/Authetication/request-by-apikey";
        }
    }
}
