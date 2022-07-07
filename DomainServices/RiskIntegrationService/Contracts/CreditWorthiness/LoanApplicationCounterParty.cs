namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
public sealed class LoanApplicationCounterParty
{
    [ProtoMember(1)]
    public string? IdMp { get; set; }

    [ProtoMember(2)]
    public bool IsPartnerMp { get; set; }

    [ProtoMember(3)]
    public int? MaritalStatusMp { get; set; }

    [ProtoMember(4)]
    public List<LoanApplicationIncome>? LoanApplicationIncome { get; set; }

    [ProtoMember(5)]
    public List<CreditLiability>? CreditLiabilities { get; set; }
}
