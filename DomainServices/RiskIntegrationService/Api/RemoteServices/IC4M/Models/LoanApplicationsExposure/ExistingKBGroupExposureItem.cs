using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// ExistingKBGroupExposureItem 
    /// </summary>
    public class ExistingKBGroupExposureItem 
    {
        /// <summary>
        /// productId
        /// </summary>
        public ResourceIdentifier ProductId { get; set; }

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
        /// drawingAmount
        /// </summary>
        public Amount DrawingAmount { get; set; }

        /// <summary>
        /// contractDate
        /// </summary>
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// maturityDate
        /// </summary>
        public DateTime? MaturityDate { get; set; }

        /// <summary>
        /// loanBalanceAmount
        /// </summary>
        public Amount LoanBalanceAmount { get; set; }

        /// <summary>
        /// loanOnBalanceAmount
        /// </summary>
        public Amount LoanOnBalanceAmount { get; set; }

        /// <summary>
        /// loanOffBalanceAmount
        /// </summary>
        public Amount LoanOffBalanceAmount { get; set; }

        /// <summary>
        /// noOfDaysPastDue
        /// </summary>
        public int? NoOfDaysPastDue { get; set; }

        /// <summary>
        /// loanAmountPastDue
        /// </summary>
        public Amount LoanAmountPastDue { get; set; }

        /// <summary>
        /// exposureAmount
        /// </summary>
        public Amount ExposureAmount { get; set; }

        /// <summary>
        /// installmentAmount
        /// </summary>
        public Amount InstallmentAmount { get; set; }

        /// <summary>
        /// isSecured
        /// </summary>
        public bool? IsSecured { get; set; }
    }
}
