namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.Shared
{
    /// <summary>
    /// OrganizationUnit
    /// </summary>
    public class OrganizationUnit
    {
        /// <summary>
        /// Pobočka + expozitura přihlášení uživatele/schvalovatele
        /// </summary>
        /// <value>Pobočka + expozitura přihlášení uživatele/schvalovatele</value>
        public string Id { get; set; }

        /// <summary>
        /// Název pobočky přihlášení uživatele/schvalovatele
        /// </summary>
        /// <value>Název pobočky přihlášení uživatele/schvalovatele</value>
        public string Name { get; set; }

        /// <summary>
        /// Pracovní skupina
        /// </summary>
        /// <value>Pracovní skupina</value>
        public JobPost JobPost { get; set; }
    }
}