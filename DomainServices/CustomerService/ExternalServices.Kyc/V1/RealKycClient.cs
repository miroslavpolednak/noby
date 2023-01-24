namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

internal sealed class RealKycClient
    : IKycClient
{
    public async Task SetSocialCharacteristics(int kbCustomerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/public/v2/customers/{kbCustomerId}/social-characteristics", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Contracts.Error>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(SetSocialCharacteristics), nameof(Contracts.Error));

            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {error.Message}, {error.Detail}");
        }
    }

    private readonly HttpClient _httpClient;
    public RealKycClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
