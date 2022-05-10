namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Důvod(y) nespočetní výseldků bonity.
    /// </summary>
    public class ResultReason
    {
        /// <summary>
        /// kód důvodu nespočtení výsledku
        /// </summary>
        /// <value>kód důvodu nespočtení výsledku</value>
        public string Code { get; set; }

        /// <summary>
        /// popis důvodu nespočtení výsledku
        /// </summary>
        /// <value>popis důvodu nespočtení výsledku</value>
        public string Description { get; set; }
    }
}
