using System;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationEntrepreneurIncome
    /// </summary>
    public class LoanApplicationEntrepreneurIncome
    {
        
        /// <summary>
        /// ID_DI FOP.
        /// </summary>
        /// <value>ID_DI FOP.</value>
        public ResourceIdentifier EntrepreneurId { get; set; }
        

        /// <summary>
        /// rodné číslo / IČO FOP.
        /// </summary>
        /// <value>rodné číslo / IČO FOP.</value>
        public string EntrepreneurIdentificationNumber { get; set; }

        /// <summary>
        /// agregovaný OKEČ.
        /// </summary>
        /// <value>agregovaný OKEČ.</value>
        public long? Nace { get; set; }

        /// <summary>
        /// povolání.
        /// </summary>
        /// <value>povolání.</value>
        public long? Profession { get; set; }

        /// <summary>
        /// PSČ sídla podnikání.
        /// </summary>
        /// <value>PSČ sídla podnikání.</value>
        public long? Postcode { get; set; }

        /// <summary>
        /// město sídla podnikání.
        /// </summary>
        /// <value>město sídla podnikání.</value>
        public string City { get; set; }

        /// <summary>
        /// země sídla podnikání.
        /// </summary>
        /// <value>země sídla podnikání.</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// establishedOn
        /// </summary>
        /// <value>establishedOn</value>
        public DateTime? EstablishedOn { get; set; }

        /// <summary>
        /// Příznak, zdali je příjem domicilován.
        /// </summary>
        /// <value>Příznak, zdali je příjem domicilován.</value>
        public bool? Domiciled { get; set; }

        /// <summary>
        /// dokument dokládající příjem z pronájmu.
        /// </summary>
        /// <value>dokument dokládající příjem z pronájmu.</value>
        public string ProofType { get; set; }

        /// <summary>
        /// čisté příjmy z podnikatelské činnosti (čistý roční příjem z daňového přiznání).
        /// </summary>
        /// <value>čisté příjmy z podnikatelské činnosti (čistý roční příjem z daňového přiznání).</value>
        public Amount AnnualIncomeAmount { get; set; }
    }
}
