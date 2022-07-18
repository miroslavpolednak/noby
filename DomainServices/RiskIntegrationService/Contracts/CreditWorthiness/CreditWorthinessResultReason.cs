namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
public sealed class CreditWorthinessResultReason
{
    [ProtoMember(1)]
    public string? Code { get; set; }

    [ProtoMember(2)]
    public string? Description { get; set; }
}
