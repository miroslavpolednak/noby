namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationDeclaredSecuredProduct
{
    [ProtoMember(1)]
    public string? ProductClusterCode { get; set; }

    [ProtoMember(2)]
    public string? AplType { get; set; }

    [ProtoMember(3)]
    public decimal? RemainingExposure { get; set; }
}
