using ExternalServices.Rip.V1.RipWrapper;

namespace ExternalServices.Rip.V1
{
    internal sealed class MockRipClient : IRipClient
    {
        public async Task<IServiceCallResult> CreateRiskBusinesCase(int salesArrangementId, string resourceProcessId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult<string>("123"));
        }

        public async Task<IServiceCallResult> ComputeCreditWorthiness(CreditWorthinessCalculationArguments arguments)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult<CreditWorthinessCalculation>( new() { InstallmentLimit = 22541, MaxAmount = 4143716, RemainsLivingAnnuity = 25450, RemainsLivingInst = 7896, WorthinessResult = 1 }));
        }

        public async Task<IServiceCallResult> CreateLoanApplication(LoanApplicationRequest arguments)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult<string>("123"));
        }

        public async Task<IServiceCallResult> GetLoanApplication(string loanApplicationAssessmentId, List<string> expand)
        {
            return await Task.FromResult(new ErrorServiceCallResult(0, "chyba"));
        }
    }
}