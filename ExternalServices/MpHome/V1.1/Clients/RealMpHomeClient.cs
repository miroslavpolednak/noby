using CIS.Infrastructure.Logging;
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

        
        public async Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateLoanPartnerLink with loanId {loanId}, partnerId {partnerId}, LoanLinkRequest {loanLinkRequest}", loanId, partnerId, System.Text.Json.JsonSerializer.Serialize(loanLinkRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateLoanPartnerLinkAsync( loanId, partnerId, loanLinkRequest);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome DeletePartnerLoanLink with loanId {loanId}, partnerId {partnerId}", loanId, partnerId);

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsDeletePartnerLoanLinkAsync(loanId, partnerId);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public Task<IServiceCallResult> UpdatePartner(long partnerId, PartnerRequest request)
        {
            _logger.LogSerializedObject($"Run inputs: MpHome UpdatePartner #{partnerId}", request);

            return WithClient(async c =>
            {
                return await callMethod(async () =>
                {
                    await c.FomsUpdatePartnerAsync(partnerId, request);

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
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ErrorServiceCallResult(9402, $"Error occured during call external service MpHome [{e.Message}]"); //TODO: error code
            }
        }
    }
}