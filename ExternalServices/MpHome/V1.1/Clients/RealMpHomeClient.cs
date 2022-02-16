using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1._1
{
    internal sealed class RealMpHomeClient : BaseClient<RealMpHomeClient>, IMpHomeClient
    {
        public RealMpHomeClient(HttpClient httpClient, ILogger<RealMpHomeClient> logger) : base(httpClient, logger) { }

      
        public async Task<IServiceCallResult> UpdateLoan(long loanId, MortgageRequest mortgageRequest)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateLoan with loanId {loanId}, MortgageRequest {mortgageRequest}", loanId, System.Text.Json.JsonSerializer.Serialize(mortgageRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateLoanAsync(loanId, mortgageRequest);

                    return new SuccessfulServiceCallResult();
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
            catch (ApiException<ModelErrorWrapper> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<ModelErrorWrapper>>(ex);
            }
            catch (ApiException<Error> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<Error>>(ex);
            }
            catch (ApiException<ProblemDetails> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<ProblemDetails>>(ex);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException>(ex);
            }
        }
    }
}
