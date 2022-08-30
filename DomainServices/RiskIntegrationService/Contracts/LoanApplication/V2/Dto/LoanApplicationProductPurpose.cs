namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationProductPurpose
{
    [ProtoMember(1)]
    public int LoanPurposeId { get; set; }

    [ProtoMember(2)]
    public decimal Amount { get; set; }
}
