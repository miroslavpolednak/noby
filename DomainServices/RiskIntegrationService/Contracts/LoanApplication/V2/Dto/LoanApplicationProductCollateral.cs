namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationProductCollateral
{
    [ProtoMember(1)]
    public string? Id { get; set; }

    [ProtoMember(2)]
    public int? CollateralType { get; set; }

    [ProtoMember(3)]
    public Shared.AmountDetail? AppraisedValue { get; set; }
}
