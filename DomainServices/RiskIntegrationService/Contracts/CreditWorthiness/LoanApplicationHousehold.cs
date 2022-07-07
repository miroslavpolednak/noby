namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
public sealed class LoanApplicationHousehold
{
    [ProtoMember(1)]
    public int ChildrenUnderAnd10 { get; set; }

    [ProtoMember(2)]
    public int ChildrenOver10 { get; set; }

    [ProtoMember(3)]
    public ExpensesSummary? ExpensesSummary { get; set; }

    [ProtoMember(4)]
    public List<LoanApplicationCounterParty>? Clients { get; set; }
}
