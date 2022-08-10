namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class LoanApplicationScore
{
    [ProtoMember(1)]
    public string? Value { get; set; }

    [ProtoMember(2)]
    public string? Scale { get; set; }
}
