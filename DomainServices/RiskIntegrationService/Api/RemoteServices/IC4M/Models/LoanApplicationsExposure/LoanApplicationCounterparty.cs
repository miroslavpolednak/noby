using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// LoanApplicationCounterparty
    /// </summary>
    public class LoanApplicationCounterparty 
    {
        /// <summary>
        /// loanApplicationCounterpartyId
        /// </summary>
        public long? LoanApplicationCounterpartyId { get; set; }

        /// <summary>
        /// customerId
        /// </summary>
        public ResourceIdentifier CustomerId { get; set; }

        /// <summary>
        /// roleCode
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// cbcbRegiterCalled
        /// </summary>
        public bool? CbcbRegiterCalled { get; set; }

        /// <summary>
        /// cbcbReportId
        /// </summary>
        public ResourceIdentifier CbcbReportId { get; set; }

        /// <summary>
        /// existingKBGroupNaturalPersonExposureItem
        /// </summary>
        public List<ExistingKBGroupExposureItem> ExistingKBGroupNaturalPersonExposureItem { get; set; }

        /// <summary>
        /// existingKBGroupJuridicalPersonExposureItem
        /// </summary>
        public List<ExistingKBGroupExposureItem> ExistingKBGroupJuridicalPersonExposureItem { get; set; }

        /// <summary>
        /// requestedKBGroupNaturalPersonExposureItem
        /// </summary>
        public List<RequestedKBGroupExposureItem> RequestedKBGroupNaturalPersonExposureItem { get; set; }

        /// <summary>
        /// requestedKBGroupJuridicalPersonExposureItem
        /// </summary>
        public List<RequestedKBGroupExposureItem> RequestedKBGroupJuridicalPersonExposureItem { get; set; }

        /// <summary>
        /// existingCBCBNaturalPersonExposureItem
        /// </summary>
        public List<ExistingCBCBExposureItem> ExistingCBCBNaturalPersonExposureItem { get; set; }

        /// <summary>
        /// existingCBCBJuridicalPersonExposureItem
        /// </summary>
        public List<ExistingCBCBExposureItem> ExistingCBCBJuridicalPersonExposureItem { get; set; }

        /// <summary>
        /// requestedCBCBNaturalPersonExposureItem
        /// </summary>
        public List<RequestedCBCBExposureItem> RequestedCBCBNaturalPersonExposureItem { get; set; }

        /// <summary>
        /// requestedCBCBJuridicalPersonExposureItem
        /// </summary>
        public List<RequestedCBCBExposureItem> RequestedCBCBJuridicalPersonExposureItem { get; set; }
    }
}
