namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
public class BankAccountDetail
{
    [ProtoMember(1)]
    public string? NumberPrefix { get; set; }

    [ProtoMember(2)]
    public string? Number { get; set; }

    [ProtoMember(3)]
    public string? BankCode { get; set; }
}
