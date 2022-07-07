namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
public sealed class ResultReason
{
    [ProtoMember(1)]
    public string? Code { get; set; }

    [ProtoMember(2)]
    public string? Description { get; set; }
}
