
using System;
using System.Collections.Generic;


namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// Change
    /// </summary>

    public class Change
    {
        /// <summary>
        /// Identita actora/uživatele
        /// </summary>
        /// <value>Identita actora/uživatele</value>
        public string IdentityId { get; set; }

        /// <summary>
        /// Datum změny
        /// </summary>
        /// <value>Datum změny</value>
        public DateTime? Date { get; set; }

    }

}
  
