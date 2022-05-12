using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationIncome
    /// </summary>
    public class LoanApplicationIncome
    {
        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public bool? IncomeConfirmed { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public bool? IncomeCollected { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public DateTime? LastConfirmedDate { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public List<LoanApplicationEmploymentIncome> EmploymentIncome { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public List<LoanApplicationEntrepreneurIncome> EntrepreneurIncome { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public LoanApplicationRentIncome RentIncome { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public List<LoanApplicationOtherIncome> OtherIncome { get; set; }
    }
}
