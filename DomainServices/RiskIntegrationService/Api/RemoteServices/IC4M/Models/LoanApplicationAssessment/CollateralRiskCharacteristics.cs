namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{
    /// <summary>
    /// CollateralRiskCharacteristics
    /// </summary>
    public class CollateralRiskCharacteristics
    {
        /// <summary>
        /// Ltv
        /// </summary>
        public long? Ltv { get; set; }

        /// <summary>
        /// Ltfv
        /// </summary>
        public long? Ltfv { get; set; }

        /// <summary>
        /// Ltp
        /// </summary>
        public long? Ltp { get; set; }

        /// <summary>
        /// SumAppraisedValue
        /// </summary>
        public long? SumAppraisedValue { get; set; }
    }
}
