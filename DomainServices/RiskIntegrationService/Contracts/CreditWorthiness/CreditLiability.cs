namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class CreditLiability
{
    [DataMember(Order = 1)]
    public int LiabilityType { get; set; }

    [DataMember(Order = 2)]
    public decimal? Limit { get; set; }

    [DataMember(Order = 3)]
    public decimal? AmountConsolidated { get; set; }

    [DataMember(Order = 4)]
    public decimal? Installment { get; set; }

    [DataMember(Order = 5)]
    public decimal? InstallmentConsolidated { get; set; }

    [DataMember(Order = 6)]
    public bool OutHomeCompanyFlag { get; set; }
}
