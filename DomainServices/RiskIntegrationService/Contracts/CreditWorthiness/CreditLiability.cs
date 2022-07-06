namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class CreditLiability
{
    [DataMember(Order = 1)]
    public int LiabilityType { get; set; }

    [DataMember(Order = 2)]
    public double? Limit { get; set; }

    [DataMember(Order = 3)]
    public double? AmountConsolidated { get; set; }

    [DataMember(Order = 4)]
    public double? Installment { get; set; }

    [DataMember(Order = 5)]
    public double? InstallmentConsolidated { get; set; }

    [DataMember(Order = 6)]
    public bool OutHomeCompanyFlag { get; set; }
}
