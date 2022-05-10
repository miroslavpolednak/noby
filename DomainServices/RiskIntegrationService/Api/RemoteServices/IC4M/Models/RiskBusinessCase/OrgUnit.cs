using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// OrgUnit
    /// </summary>
    [DataContract]
    public class OrgUnit
    {
        /// <summary>
        /// pobočka + expozitura přihlášení uživatele / schvalovatele.
        /// </summary>
        /// <value>pobočka + expozitura přihlášení uživatele / schvalovatele.</value>
        public long? Id { get; set; }

        /// <summary>
        /// název pobočky přihlášení  uživatele / schvalovatele 
        /// </summary>
        /// <value>název pobočky přihlášení  uživatele / schvalovatele </value>
        public string Name { get; set; }

        /// <summary>
        /// kód pracovní pozice přihlášeného uživatele /schvalovatele
        /// </summary>
        /// <value>kód pracovní pozice přihlášeného uživatele /schvalovatele</value>
        public JobPost JobPost { get; set; }
    }
}
