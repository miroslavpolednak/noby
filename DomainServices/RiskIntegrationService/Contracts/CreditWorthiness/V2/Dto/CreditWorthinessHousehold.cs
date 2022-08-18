namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessHousehold
{
    [ProtoMember(1)]
    public int ChildrenUpToTenYearsCount { get; set; }

    [ProtoMember(2)]
    public int ChildrenOverTenYearsCount { get; set; }

    [ProtoMember(3)]
    public Shared.V1.ExpensesSummary? ExpensesSummary { get; set; }

    [ProtoMember(4)]
    public List<CreditWorthinessCustomer>? Customers { get; set; }
}
