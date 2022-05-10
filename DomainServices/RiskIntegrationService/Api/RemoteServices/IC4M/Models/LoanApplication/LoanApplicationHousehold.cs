using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationHousehold
    {
        /// <summary>
        /// LoanApplicationHousehold identity
        /// </summary>
        /// <value>LoanApplicationHousehold identity</value>
        public long? Id { get; set; }

        /// <summary>
        /// LoanApplication index
        /// </summary>
        /// <value>LoanApplication index</value>
        public long? Index { get; set; }

        /// <summary>
        /// počet vyživovaných dětí do 10 let (včetně).
        /// </summary>
        /// <value>počet vyživovaných dětí do 10 let (včetně).</value>
        public string RoleCode { get; set; }

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
        public List<ExpensesSummary> HouseholdExpensesSummary { get; set; }

               
        /// <summary>
        /// Shrnutí pasiv úvěru domácnosti
        /// </summary>
        /// <value>Shrnutí pasiv úvěru domácnosti</value>
        public List<CreditLiabilitiesSummary> HouseholdCreditLiabilitiesSummaryOutHomeCompany { get; set; }
        
        /// <summary>
        /// Shrnutí pasiv úvěru mimo domácnosti
        /// </summary>
        /// <value>Shrnutí pasiv úvěru mimo domácnosti</value>
        public List<LoanInstallmentsSummary> HouseholdInstallmentsSummaryOutHomeCompany { get; set; }

        /// <summary>
        /// Shrnutí splátek mimo domácnosti
        /// Kód vypořádání majetku (manželů )
        /// </summary>
        /// <value>Shrnutí splátek mimo domácnosti</value>
        public string SettlementTypeCode { get; set; }

        /// <summary>
        /// Shrnutí splátek domácnosti
        /// </summary>
        /// <value>Shrnutí splátek domácnosti</value>
        public List<LoanApplicationCounterParty> CounterParty { get; set; }
    }
}
