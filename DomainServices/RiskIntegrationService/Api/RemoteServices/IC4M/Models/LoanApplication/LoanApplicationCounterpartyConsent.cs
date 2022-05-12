using System;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationCounterpartyConsent
    /// </summary>
    public class LoanApplicationCounterpartyConsent
    {

        /// <summary>
        /// Externí registr.
        /// </summary>
        /// <value>Externí registr.</value>
        public string ExternalRegisterType { get; set; }

        /// <summary>
        /// udělen souhlas s dotazem do registru NRKI?
        /// </summary>
        /// <value>udělen souhlas s dotazem do registru NRKI?</value>
        public bool? Consent { get; set; }

        /// <summary>
        /// datum udělení/neudělení souhlasu s dotazem do registru NRKI.
        /// </summary>
        /// <value>datum udělení/neudělení souhlasu s dotazem do registru NRKI.</value>
        public DateTime? ConsentDate { get; set; }

        /// <summary>
        /// loanApplicationConsentValue.
        /// </summary>
        /// <value>loanApplicationConsentValue.</value>
        public LoanApplicationConsentValue LoanApplicationConsentValue { get; set; }
    }
}
