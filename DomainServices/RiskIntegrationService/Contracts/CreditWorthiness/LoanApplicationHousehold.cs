namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class LoanApplicationHousehold
{
    [DataMember(Order = 1)]
    public int? ChildrenUnderAnd10 { get; set; }

    [DataMember(Order = 2)]
    public int? ChildrenOver10 { get; set; }

    [DataMember(Order = 3)]
    public ExpensesSummary? ExpensesSummary { get; set; }

    [DataMember(Order = 4)]
    public List<LoanApplicationCounterParty>? Clients { get; set; }
}
