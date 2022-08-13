namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
public sealed class ResultReasonDetail
{
    [ProtoMember(1)]
    public string? Code { get; set; }

    [ProtoMember(2)]
    public string? Description { get; set; }
}
