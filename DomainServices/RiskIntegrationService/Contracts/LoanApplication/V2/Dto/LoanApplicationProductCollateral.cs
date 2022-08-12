namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationProductCollateral
{
    [ProtoMember(1)]
    public string? Id { get; set; }

    [ProtoMember(2)]
    public int? CollateralTypeId { get; set; }

    [ProtoMember(3)]
    public decimal? AppraisedValue { get; set; }
}
