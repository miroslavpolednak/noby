namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class LoanApplicationCounterParty
{
    [DataMember(Order = 1)]
    public string? IdMp { get; set; }

    [DataMember(Order = 2)]
    public bool IsPartnerMp { get; set; }

    [Required]
    [DataMember(Order = 3)]
    public int? MaritalStatusMp { get; set; }

    [DataMember(Order = 4)]
    public List<LoanApplicationIncome>? LoanApplicationIncome { get; set; }

    [DataMember(Order = 5)]
    public List<CreditLiability>? CreditLiabilities { get; set; }
}
