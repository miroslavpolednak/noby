namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationHousehold
{
    [ProtoMember(1)]
    public int HouseholdId { get; set; }

    [ProtoMember(2)]
    public int? HouseholdTypeId { get; set; }

    [ProtoMember(3)]
    public int? ChildrenUpToTenYearsCount { get; set; }

    [ProtoMember(4)]
    public int? ChildrenOverTenYearsCount { get; set; }

    [ProtoMember(5)]
    public Shared.ExpensesSummary.V1.ExpensesSummary? Expenses { get; set; }

    [ProtoMember(6)]
    public int? PropertySettlementId { get; set; }

    [ProtoMember(7)]
    public List<LoanApplicationCustomer>? Customers { get; set; }
}
