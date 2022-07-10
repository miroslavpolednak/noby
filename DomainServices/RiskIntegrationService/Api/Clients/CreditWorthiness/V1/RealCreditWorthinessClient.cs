using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        _logger.ExtServiceRequest(CreditWorthinessStartupExtensions.ServiceName, _calculateUrl, request);

        return await HttpClientResponseHelper.CallService(
            async () => await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken),
            async (response) =>
            {
                var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculation>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, _calculateUrl, nameof(CreditWorthinessCalculation));

                _logger.ExtServiceResponse(CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), response);
                return result;
            },
            CreditWorthinessStartupExtensions.ServiceName,
            nameof(Calculate));
    }

    private readonly HttpClient _httpClient;
    private readonly ILogger<RealCreditWorthinessClient> _logger;

    const string _calculateUrl = "/riskBusinessCase";

    public RealCreditWorthinessClient(HttpClient httpClient, ILogger<RealCreditWorthinessClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
