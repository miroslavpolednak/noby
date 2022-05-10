namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationRentIncome
    /// </summary>
    public class LoanApplicationRentIncome
    {
        /// <summary>
        /// accountNumber.
        /// </summary>
        /// <value>accountNumber.</value>
        public string AccountNumber { get; set; }

        /// <summary>
        /// domiciled.
        /// </summary>
        /// <value>domiciled.</value>
        public bool? Domiciled { get; set; }

        /// <summary>
        /// proofType.
        /// </summary>
        /// <value>proofType.</value>
        public string ProofType { get; set; }

        /// <summary>
        /// čisté měsíční příjmy z pronájmu.
        /// </summary>
        /// <value>čisté měsíční příjmy z pronájmu.</value>
        public Amount MonthlyIncomeAmount { get; set; }
    }
}
