namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

internal sealed class RealKycClient
    : IKycClient
{
    public async Task SetSocialCharacteristics(int kbCustomerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await sendRequest($"/public/v2/customers/{kbCustomerId}/social-characteristics", request, cancellationToken);
    }

    public async Task SetKyc(int kbCustomerId, Contracts.Kyc request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await sendRequest($"/public/v2/customers/{kbCustomerId}/kyc", request, cancellationToken);
    }

    public async Task SetFinancialProfile(int kbCustomerId, Contracts.EmploymentFinancialProfile request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await sendRequest($"/public/v2/customers/{kbCustomerId}/employment-financial-profile", request, cancellationToken);
    }

    private async Task sendRequest<TRequest>(string url, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + url, request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Contracts.Error>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(TRequest), nameof(Contracts.Error));

            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {error.Message}, {error.Detail}");
        }
    }

    private readonly HttpClient _httpClient;
    public RealKycClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
