using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    ///ExistingCBCBExposureItem 
    /// </summary>
    public class ExistingCBCBExposureItem
    {
        /// <summary>
        /// cbcbContractId
        /// </summary>
        public ResourceIdentifier CbcbContractId { get; set; }

        /// <summary>
        /// customerRoleCode
        /// </summary>
        public string CustomerRoleCode { get; set; }

        /// <summary>
        /// loanType
        /// </summary>
        public string LoanType { get; set; }

        /// <summary>
        /// maturityDate
        /// </summary>
        public DateTime? MaturityDate { get; set; }

        /// <summary>
        /// loanAmount
        /// </summary>
        public Amount LoanAmount { get; set; }

        /// <summary>
        /// installmentAmount
        /// </summary>
        public Amount InstallmentAmount { get; set; }

        /// <summary>
        /// exposureAmount
        /// </summary>
        public Amount ExposureAmount { get; set; }

        /// <summary>
        /// contractDate
        /// </summary>
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// cbcbDataLastUpdate
        /// </summary>
        public DateTime? CbcbDataLastUpdate { get; set; }
    }
}
