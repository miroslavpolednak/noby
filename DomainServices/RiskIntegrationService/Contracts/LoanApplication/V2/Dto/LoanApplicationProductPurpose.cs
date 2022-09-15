namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationProductPurpose
{
    [ProtoMember(1)]
    public int LoanPurposeId { get; set; }

    [ProtoMember(2)]
    public decimal Amount { get; set; }
}
