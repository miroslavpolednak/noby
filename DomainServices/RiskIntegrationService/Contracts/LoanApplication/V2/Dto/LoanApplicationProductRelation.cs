namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationProductRelation
{
    [ProtoMember(1)]
    public string? CbcbContractId { get; set; }

    [ProtoMember(2)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(3)]
    public decimal? RemainingExposure { get; set; }

    [ProtoMember(4)]
    public string ProductType { get; set; } = null!;

    [ProtoMember(5)]
    public string RelationType { get; set; } = null!;

    [ProtoMember(6)]
    public List<LoanApplicationProductRelationCustomer>? Customers { get; set; }
}
