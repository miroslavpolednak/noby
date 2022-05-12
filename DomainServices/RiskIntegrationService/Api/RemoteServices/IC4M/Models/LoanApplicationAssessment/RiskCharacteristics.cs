namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// RiskCharacteristics
    /// </summary>
    public class RiskCharacteristics
    {
        /// <summary>
        /// monthlyIncomeAmount
        /// </summary>
        public Amount MonthlyIncomeAmount { get; set; }

        /// <summary>
        /// MonthlyCostsWithoutInstAmount
        /// </summary>
        public Amount MonthlyCostsWithoutInstAmount { get; set; }

        /// <summary>
        /// MonthlyCostsWithoutInstAmount
        /// </summary>
        public Amount MonthlyInstallmentsInKBAmount { get; set; }

        /// <summary>
        /// MonhlyEntrepreneurInstallmentsInKBAmount
        /// </summary>
        public Amount MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }

        /// <summary>
        /// MonhlyInstallmentsInMPSSAmount
        /// </summary>
        public Amount MonthlyInstallmentsInMPSSAmount { get; set; }

        /// <summary>
        /// MonhlyInstallmentsInOFIAmount
        /// </summary>
        public Amount MonthlyInstallmentsInOFIAmount { get; set; }

        /// <summary>
        /// MonhlyInstallmentsInCBCBAmount
        /// </summary>
        public Amount MonthlyInstallmentsInCBCBAmount { get; set; }
    }
}
