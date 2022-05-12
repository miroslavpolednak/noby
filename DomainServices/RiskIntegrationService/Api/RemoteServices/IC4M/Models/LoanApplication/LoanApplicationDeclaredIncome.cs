using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    public class LoanApplicationDeclaredIncome
    {
        /// <summary>
        /// employer
        /// </summary>
        /// <value>employer</value>
        public List<LoanApplicationDeclaredEmploymentIncome> Employer { get; set; }

        /// <summary>
        /// entrepreneurAnnualIncomeAmount
        /// </summary>
        /// <value>entrepreneurAnnualIncomeAmount</value>
        public Amount EntrepreneurAnnualIncomeAmount { get; set; }

        /// <summary>
        /// monthlyRentIncomeAmount
        /// </summary>
        /// <value>monthlyRentIncomeAmount</value>
        public Amount MonthlyRentIncomeAmount { get; set; }


    }
}
