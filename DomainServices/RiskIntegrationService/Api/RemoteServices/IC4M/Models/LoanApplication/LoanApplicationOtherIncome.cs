namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplicationOtherIncome
    /// </summary>
    public class LoanApplicationOtherIncome
    {
        /// <summary>
        /// Typ  vedlejšího prokazatelného příjmu. Opakující se atribut podle počtu vedeljších prokazatelných příjmů příjmů.
        /// </summary>
        /// <value>Typ  vedlejšího prokazatelného příjmu. Opakující se atribut podle počtu vedeljších prokazatelných příjmů příjmů.</value>
        public string Type { get; set; }


        /// <summary>
        /// Částka  vedlejšího prokazatelného příjmu. Opakující se atribut podle počtu vedeljších prokazatelných příjmů příjmů.
        /// </summary>
        /// <value>Částka  vedlejšího prokazatelného příjmu. Opakující se atribut podle počtu vedeljších prokazatelných příjmů příjmů.</value>
        public decimal? MonthlyIncomeAmount { get; set; }
        
        
        /// <summary>
        /// číslo účtu, z kterého příjem přichází.
        /// </summary>
        /// <value>číslo účtu, z kterého příjem přichází.</value>
        public string AccountNumber { get; set; }

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
    }
}
