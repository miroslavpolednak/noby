using ExternalServices.Rip.V1.RipWrapper;
using CIS.Infrastructure.Logging;

namespace ExternalServices.Rip.V1
{
    internal sealed class RealRipClient : BaseClient<RealRipClient>, IRipClient
    {
        public RealRipClient(HttpClient httpClient, ILogger<RealRipClient> logger) : base(httpClient, logger) { }

        public async Task<IServiceCallResult> CreateRiskBusinesCase(int salesArrangementId, string resourceProcessId)
        {
            _logger.LogDebug("Run inputs: Rip CreateRiskBusinesCase with {SAid} {RpId}", salesArrangementId, resourceProcessId);

            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.RiskBusinessCaseCreateAsync(new CreateRequest
                    {
                        LoanApplicationIdMp = new SystemId
                        {
                            Id = salesArrangementId.ToString(),
                            System = "NOBY"
                        },
                        ItChannel = "NOBY",
                        ResourceProcessIdMp = resourceProcessId
                    });
                    
                    return new SuccessfulServiceCallResult<string>(result.RiskBusinessCaseIdMp);
                });

            });
        }

        public async Task<IServiceCallResult> ComputeCreditWorthiness(CreditWorthinessCalculationArguments arguments)
        {
            _logger.LogSerializedObject("ComputeCreditWorthiness", arguments);

            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.ComputeCreditWorthinessAsync(arguments);

                    return new SuccessfulServiceCallResult<CreditWorthinessCalculation>(result);
                });

            });
        }

        public async Task<IServiceCallResult> CreateLoanApplication(LoanApplicationRequest arguments)
        {
            _logger.LogSerializedObject("CreateLoanApplication", arguments);
            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.CreateLoanApplicationAsync(arguments);

                    return new SuccessfulServiceCallResult<string>(result.RiskSegment);
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
