using System;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// PhoneContact
    /// </summary>
    public class PrimaryAddress
    {
       
        /// <summary>
        /// Ulice.
        /// </summary>
        /// <value>Ulice.</value>
        public string Street { get; set; }

        /// <summary>
        /// Město.
        /// </summary>
        /// <value>Město.</value>
        public string City { get; set; }

        /// <summary>
        /// Země.
        /// </summary>
        /// <value>Země.</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Směrovací číslo.
        /// </summary>
        /// <value>Směrovací číslo.</value>
        public long? PostCode { get; set; }

        /// <summary>
        /// Region.
        /// </summary>
        /// <value>Region.</value>
        public long? RegionCode { get; set; }

        /// <summary>
        /// inhabitancyFrom
        /// </summary>
        /// <value>inhabitancyFrom</value>
        public DateTime? InhabitancyFrom { get; set; }
    }
}
