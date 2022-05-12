using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Protistrana žádosti o půjčku.
    /// </summary>
    public class LoanApplicationCounterParty
    {
        /// <summary>
        /// Identifikátor klienta (např. v případě KB klienta IDDI)
        /// </summary>
        /// <value>Identifikátor klienta (např. v případě KB klienta IDDI)</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// Gets or Sets LoanApplicationIncome
        /// </summary>
        public List<LoanApplicationIncome> LoanApplicationIncome { get; set; }

        /// <summary>
        /// současný rodinný stav
        /// </summary>
        /// <value>současný rodinný stav</value>
        public int? IsPartner { get; set; }

        /// <summary>
        /// Je klient druhem/družkou?
        /// </summary>
        /// <value>Je klient druhem/družkou?</value>
        public string MaritalStatus { get; set; }
    }
}
