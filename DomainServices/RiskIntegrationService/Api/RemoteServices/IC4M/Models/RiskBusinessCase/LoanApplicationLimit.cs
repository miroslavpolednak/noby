using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanApplicationLimit
    {
        /// <summary>
        /// loanApplicationLimit.
        /// </summary>
        /// <value>loanApplicationLimit.</value>
        public Amount _LoanApplicationLimit { get; set; }

        /// <summary>
        /// loanApplicationInstallmentLimit.
        /// </summary>
        /// <value>loanApplicationInstallmentLimit.</value>
        public Amount LoanApplicationInstallmentLimit { get; set; }

        /// <summary>
        /// loanApplicationCollateralLimit.
        /// </summary>
        /// <value>loanApplicationCollateralLimit.</value>
        public Amount LoanApplicationCollateralLimit { get; set; }

        /// <summary>
        /// remainingAnnuityLivingAmount.
        /// </summary>
        /// <value>remainingAnnuityLivingAmount.</value>
        public Amount RemainingAnnuityLivingAmount { get; set; }
    }
}
