namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// LoanApplicationLimit
    /// </summary>
    public class LoanApplicationLimit
    {
        /// <summary>
        /// Loan Application Limit
        /// </summary>
        public Amount _LoanApplicationLimit { get; set; }

        /// <summary>
        /// Loan Application Installment Limit
        /// </summary>
        public Amount LoanApplicationInstallmentLimit { get; set; }

        /// <summary>
        /// Loan Application Collateral Limit
        /// </summary>
        public Amount LoanApplicationCollateralLimit { get; set; }

        /// <summary>
        /// Remaining Annuity Living Amount
        /// </summary>
        public Amount RemainingAnnuityLivingAmount { get; set; }

        /// <summary>
        /// Remaining Annuity Living Amount
        /// </summary>
        public bool? CalculationIrStressed { get; set; }

        /// <summary>
        /// iir
        /// </summary>
        public long? Iir { get; set; }

        /// <summary>
        /// cir
        /// </summary>
        public long? Cir { get; set; }

        /// <summary>
        /// dti
        /// </summary>
        public long? Dti { get; set; }

        /// <summary>
        /// dsti
        /// </summary>
        public long? Dsti { get; set; }
    }
}
