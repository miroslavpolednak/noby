using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    public class LoanApplicationDeclaredEmploymentIncome
    {
        
        /// <summary>
        /// employerId
        /// </summary>
        /// <value>employerId</value>
        public ResourceIdentifier EmployerId { get; set; }

        /// <summary>
        /// employerIdentificationNumber
        /// </summary>
        /// <value>employerIdentificationNumber</value>
        public string EmployerIdentificationNumber { get; set; }

        /// <summary>
        /// monthlyIncomeAmount
        /// </summary>
        /// <value>monthlyIncomeAmount</value>
        public Amount MonthlyIncomeAmount { get; set; }

        /// <summary>
        /// domiciled
        /// </summary>
        /// <value>domiciled</value>
        public bool? Domiciled { get; set; }



    }
}
