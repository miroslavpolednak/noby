using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        _logger.ExtServiceRequest(CreditWorthinessStartupExtensions.ServiceName, _calculateUrl, request);

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculation>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, _calculateUrl, nameof(CreditWorthinessCalculation));

            _logger.ExtServiceResponse(CreditWorthinessStartupExtensions.ServiceName, _calculateUrl, response);
            return result;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            // asi validacni chyba?
            throw new CisValidationException(0, "validatce");
        }
        else
        {
            // 500?
            throw new ServiceCallResultErrorException(0, "chyba?");
        }
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
