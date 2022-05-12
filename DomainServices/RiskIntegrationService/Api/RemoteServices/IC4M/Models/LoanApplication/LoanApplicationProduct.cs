using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationProduct
    {
        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public string ProductClusterCode { get; set; }

        /// <summary>
        /// z tabulek xxTABGL, sloupec C_PROD_SEL.
        /// </summary>
        /// <value>z tabulek xxTABGL, sloupec C_PROD_SEL.</value>
        public string GlTableSelection { get; set; }

        /// <summary>
        /// aplikace + typ účtu.
        /// </summary>
        /// <value>aplikace + typ účtu.</value>
        public string AplType { get; set; }

        /// <summary>
        /// účely úvěru.
        /// </summary>
        /// <value>účely úvěru.</value>
        public List<LoanApplicationPurpose> LoanApplicationPurpose { get; set; }

        /// <summary>
        /// seznam zajištění daného účtu. Zabaluje větve COLLATERAL. Shodná struktura jako v CrI_out.
        /// </summary>
        /// <value>seznam zajištění daného účtu. Zabaluje větve COLLATERAL. Shodná struktura jako v CrI_out.</value>
        public List<LoanApplicationCollateral> LoanApplicationCollateral { get; set; }

        /// <summary>
        /// požadovaná výše úvěru.
        /// </summary>
        /// <value>požadovaná výše úvěru.</value>
        public Amount AmountRequired { get; set; }

        /// <summary>
        /// celková výše investice.
        /// </summary>
        /// <value>celková výše investice.</value>
        public Amount AmountInvestment { get; set; }

        /// <summary>
        /// celková výše vlastních zdrojů.
        /// </summary>
        /// <value>celková výše vlastních zdrojů.</value>
        public Amount AmountOwnResources { get; set; }

        /// <summary>
        /// celková výše cizích zdrojů.
        /// </summary>
        /// <value>celková výše cizích zdrojů.</value>
        public Amount AmountForeignResources { get; set; }

        /// <summary>
        /// počet splátek (anuitní a úrokové).
        /// </summary>
        /// <value>počet splátek (anuitní a úrokové).</value>
        public long? Maturity { get; set; }

        /// <summary>
        /// žádaná anuitní splátka.
        /// </summary>
        /// <value>žádaná anuitní splátka.</value>
        public long? Annuity { get; set; }

        /// <summary>
        /// doba fixace úrokové sazby.
        /// </summary>
        /// <value>doba fixace úrokové sazby.</value>
        public long? FixationPeriod { get; set; }

        /// <summary>
        /// žádaná roční úroková sazba.
        /// </summary>
        /// <value>žádaná roční úroková sazba.</value>
        public long? InterestRate { get; set; }

        /// <summary>
        /// typ splátkového kalendáře.
        /// </summary>
        /// <value>typ splátkového kalendáře.</value>
        public string RepaymentScheduleType { get; set; }

        /// <summary>
        /// periodicity splátek.
        /// </summary>
        /// <value>periodicity splátek.</value>
        public string InstallmentPeriod { get; set; }

        /// <summary>
        /// počet splátek (pouze anuitní).
        /// </summary>
        /// <value>počet splátek (pouze anuitní).</value>
        public long? InstallmentCount { get; set; }

        /// <summary>
        /// termín čerpání obchodu od.
        /// </summary>
        /// <value>termín čerpání obchodu od.</value>
        public DateTime? DrawingPeriodStart { get; set; }

        /// <summary>
        /// termín čerpání obchodu do.
        /// </summary>
        /// <value>termín čerpání obchodu do.</value>
        public DateTime? DrawingPeriodEnd { get; set; }

        /// <summary>
        /// datum začátku splácení.
        /// </summary>
        /// <value>datum začátku splácení.</value>
        public DateTime? RepaymentPeriodStart { get; set; }

        /// <summary>
        /// konečný den splatnosti.
        /// </summary>
        /// <value>konečný den splatnosti.</value>
        public DateTime? RepaymentPeriodEnd { get; set; }

        /// <summary>
        /// vnitrobankovní úroková sazba.
        /// </summary>
        /// <value>vnitrobankovní úroková sazba.</value>
        public long? Vbs { get; set; }

        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>
        /// <value>kód produktového shluku (shluk jednoho produktu).</value>
        public bool? IsProductSecured { get; set; }

        /// <summary>
        /// domácí měna dle příjmu žadatelů.
        /// </summary>
        /// <value>domácí měna dle příjmu žadatelů.</value>
        public string HomeCurrencyIncome { get; set; }

        /// <summary>
        /// domácí měna dle bydliště žadatelů.
        /// </summary>
        /// <value>domácí měna dle bydliště žadatelů.</value>
        public string HomeCurrencyResidence { get; set; }
    }
}
