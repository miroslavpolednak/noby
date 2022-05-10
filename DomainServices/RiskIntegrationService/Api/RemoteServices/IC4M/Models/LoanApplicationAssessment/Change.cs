using System;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// Change
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Identita actora/uživatele
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        /// Datum změny
        /// </summary>
        public DateTime? Date { get; set; }
    }
}
