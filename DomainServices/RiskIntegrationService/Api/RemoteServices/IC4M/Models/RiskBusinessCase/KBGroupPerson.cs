using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// KbGroupPerson
    /// </summary>
    [DataContract]
    public class KBGroupPerson
    {
        /// <summary>
        /// osobní číslo přihlášeného uživatele /schvalovatele.
        /// </summary>
        /// <value>osobní číslo přihlášeného uživatele /schvalovatele.</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// příjmení přihlášeného uživatele / příjmení schvalovatele.
        /// </summary>
        /// <value>příjmení přihlášeného uživatele / příjmení schvalovatele.</value>
        public string Surname { get; set; }

        /// <summary>
        /// OrgUnit
        /// </summary>
        /// <value>OrgUnit</value>
        public OrgUnit OrgUnit { get; set; }
    }
}
