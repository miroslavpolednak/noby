using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ResultReason
    {
        /// <summary>
        /// kód důvodu nespočtení výsledku.
        /// </summary>
        /// <value>kód důvodu nespočtení výsledku.</value>
        public string Code { get; set; }

        /// <summary>
        /// popis důvodu nespočtení výsledku.
        /// </summary>
        /// <value>popis důvodu nespočtení výsledku.</value>
        public string Desc { get; set; }
    }
}
