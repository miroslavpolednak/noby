namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationProductRelation
{
    [ProtoMember(1)]
    public string? CbcbContractId { get; set; }

    [ProtoMember(2)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(3)]
    public decimal? RemainingExposure { get; set; }
}
