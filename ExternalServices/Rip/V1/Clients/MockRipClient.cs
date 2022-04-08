using ExternalServices.Rip.V1.RipWrapper;

namespace ExternalServices.Rip.V1
{
    internal sealed class MockRipClient : IRipClient
    {
        public async Task<IServiceCallResult> CreateRiskBusinesCase(CreateRequest createRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult<LoanApplicationCreate>(new() {  RiskBusinessCaseIdMp = "123" }));
        }
    }
}