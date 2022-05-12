using System;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationPersonalDocument
    /// </summary>
    public class LoanApplicationPersonalDocument
    {
        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public string Id { get; set; }

        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public string Type { get; set; }

        /// <summary>
        /// issuedOn
        /// </summary>
        /// <value>issuedOn</value>
        public DateTime? IssuedOn { get; set; }

        /// <summary>
        /// validTo
        /// </summary>
        /// <value>validTo</value>
        public DateTime? ValidTo { get; set; }
    }
}
