using ExternalServices.Rip.V1.RipWrapper;

namespace ExternalServices.Rip.V1
{
    public interface IRipClient
    {
        /// <summary>
        /// Vytvoří Risk Busines Case.
        /// </summary>
        /// <returns>string RiskBusinessCaseIdMp</returns>
        Task<IServiceCallResult> CreateRiskBusinesCase(int salesArrangementId, string resourceProcessId);

        /// <summary>
        /// Výpočet rozšířené bonity.
        /// </summary>
        Task<IServiceCallResult> ComputeCreditWorthiness(CreditWorthinessCalculationArguments arguments);

        /// <summary>
        /// ???
        /// </summary>
        /// <returns>string - RiskSegment</returns>
        Task<IServiceCallResult> CreateLoanApplication(LoanApplicationRequest arguments);
    }
}