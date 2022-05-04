using ExternalServices.Rip.V1.RipWrapper;

namespace ExternalServices.Rip.V1
{
    internal sealed class RealRipClient : BaseClient<RealRipClient>, IRipClient
    {
        public RealRipClient(HttpClient httpClient, ILogger<RealRipClient> logger) : base(httpClient, logger) { }

        public async Task<IServiceCallResult> CreateRiskBusinesCase(CreateRequest createRequest)
        {
            _logger.LogDebug("Run inputs: Rip CreateRiskBusinesCase with CreateRequest {createRequest}", System.Text.Json.JsonSerializer.Serialize(createRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.RiskBusinessCaseCreateAsync(createRequest);

                    return new SuccessfulServiceCallResult<LoanApplicationCreate>(result);
                });

            });
        }

        public async Task<IServiceCallResult> ComputeCreditWorthiness(CreditWorthinessCalculationArguments arguments)
        {
            _logger.LogDebug("Run inputs: Rip ComputeCreditWorthiness with Arguments {arguments}", System.Text.Json.JsonSerializer.Serialize(arguments));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.ComputeCreditWorthinessAsync(arguments);

                    return new SuccessfulServiceCallResult<CreditWorthinessCalculation>(result);
                });

            });
        }


        private Client CreateClient()
            => new(_httpClient?.BaseAddress?.ToString(), _httpClient);

        private async Task<IServiceCallResult> WithClient(Func<Client, Task<IServiceCallResult>> fce)
        {
            try
            {
                return await fce(CreateClient());
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorServiceCallResult(99999, $"Error occured during call external service RIP [{ex.Message}]"); //TODO: error code
            }
        }
    }
}
