using ExternalServices.C4M.CreditWorthiness.V1.Contracts;

namespace ExternalServices.C4M.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : BaseClient<CreditWorthinessClient>, ICreditWorthinessClient
{
    public async Task<IServiceCallResult> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        _logger.LogSerializedObject("CaseStateChanged", request);

        try
        {
            var client = new CreditWorthinessClient(_httpClient?.BaseAddress?.ToString(), _httpClient);

            var result = await client.CreditWorthiness_1Async(request, cancellationToken);

            return new SuccessfulServiceCallResult<CreditWorthinessCalculation>(result);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ErrorServiceCallResult(0, $"Error occured during call external service C4M.CreditWorthiness [{ex.Message}]");
        }
    }

    public RealCreditWorthinessClient(HttpClient httpClient, ILogger<CreditWorthinessClient> logger) 
        : base(httpClient, logger) { }
}
