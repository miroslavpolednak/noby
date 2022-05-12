namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Parametry domácnosti.
/// </summary>
[DataContract]
public class LoanApplicationHousehold
{
    /// <summary>
    /// Počet vyživovaných dětí do 10 let (včetně).
    /// </summary>
    [DataMember(Order = 1)]
    public long? ChildrenUnderAnd10 { get; set; }

    /// <summary>
    /// Počet vyživovaných dětí nad 10 let.
    /// </summary>
    [DataMember(Order = 2)]
    public long? ChildrenOver10 { get; set; }

    /// <summary>
    /// Household Expenses Summary
    /// </summary>
    [DataMember(Order = 3)]
    public ExpensesSummary? ExpensesSummary { get; set; }

    /// <summary>
    /// Klienti
    /// </summary>
    [DataMember(Order = 4)]
    public List<LoanApplicationCounterParty>? Clients { get; set; }
}
