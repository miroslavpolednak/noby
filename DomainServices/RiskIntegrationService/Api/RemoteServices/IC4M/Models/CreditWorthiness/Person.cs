namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Person
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Identifikátor uživatele
        /// </summary>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// příjmení přihlášeného uživatele/příjmení schvalovatele
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Pracovní skupina
        /// </summary>
        /// <value>Pracovní skupina</value>
        public OrganizationUnit OrgUnit { get; set; }
    }
}
