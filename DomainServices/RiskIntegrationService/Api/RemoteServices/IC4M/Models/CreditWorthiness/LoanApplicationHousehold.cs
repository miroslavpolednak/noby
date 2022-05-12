using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationHousehold
    {
        /// <summary>
        /// počet vyživovaných dětí do 10 let (včetně).
        /// </summary>
        /// <value>počet vyživovaných dětí do 10 let (včetně).</value>
        public long? ChildrenUnderAnd10 { get; set; }

        /// <summary>
        /// počet vyživovaných dětí nad 10 let .
        /// </summary>
        /// <value>počet vyživovaných dětí nad 10 let .</value>
        public long? ChildrenOver10 { get; set; }

        /// <summary>
        /// Household Expenses Summary
        /// </summary>
        /// <value>Household Expenses Summary</value>
        public List<ExpensesSummary> ExpensesSummary { get; set; }

        /// <summary>
        /// Shrnutí pasiv úvěru mimo domácnosti
        /// </summary>
        /// <value>Shrnutí pasiv úvěru mimo domácnosti</value>
        public List<CreditLiabilitiesSummary> CreditLiabilitiesSummaryOut { get; set; }

        /// <summary>
        /// Shrnutí pasiv úvěru domácnosti
        /// </summary>
        /// <value>Shrnutí pasiv úvěru domácnosti</value>
        public List<CreditLiabilitiesSummaryHomeCompany> CreditLiabilitiesSummary { get; set; }

        /// <summary>
        /// Shrnutí splátek mimo domácnosti
        /// </summary>
        /// <value>Shrnutí splátek mimo domácnosti</value>
        public List<InstallmentsSummaryOutHomeCompany> InstallmentsSummaryOut { get; set; }

        /// <summary>
        /// Shrnutí splátek domácnosti
        /// </summary>
        /// <value>Shrnutí splátek domácnosti</value>
        public List<InstallmentsSummaryHomeCompany> InstallmentsSummary { get; set; }

        /// <summary>
        /// Klienti
        /// </summary>
        /// <value>Klienti</value>
        public List<LoanApplicationCounterParty> Clients { get; set; }
    }
}
