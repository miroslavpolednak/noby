namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;


[ProtoContract]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public sealed class LoanApplicationException
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    [ProtoMember(1)]
    public string? Arm { get; set; }

    [ProtoMember(2)]
    public string? ReasonCode { get; set; }
}