using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// RequestedKBGroupExposureItem
    /// </summary>
    public class RequestedKBGroupExposureItem 
    {
        /// <summary>
        /// riskBusinessCaseId
        /// </summary>
        public long? RiskBusinessCaseId { get; set; }

        /// <summary>
        /// loanType
        /// </summary>
        public string LoanType { get; set; }

        /// <summary>
        /// customerRoleCode
        /// </summary>
        public string CustomerRoleCode { get; set; }

        /// <summary>
        /// diApplicationNumber
        /// </summary>
        public string DiApplicationNumber { get; set; }

        /// <summary>
        /// accType
        /// </summary>
        public string AccType { get; set; }

        /// <summary>
        /// productClusterCode
        /// </summary>
        public string ProductClusterCode { get; set; }

        /// <summary>
        /// loanAmount
        /// </summary>
        public Amount LoanAmount { get; set; }

        /// <summary>
        /// installmentAmount
        /// </summary>
        public Amount InstallmentAmount { get; set; }

        /// <summary>
        /// isSecured
        /// </summary>
        public bool? IsSecured { get; set; }

        /// <summary>
        /// statusCode
        /// </summary>
        public string StatusCode { get; set; }
    }
}
