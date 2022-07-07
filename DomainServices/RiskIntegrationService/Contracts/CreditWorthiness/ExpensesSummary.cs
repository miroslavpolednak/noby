namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class ExpensesSummary
{
    [DataMember(Order = 1)]
    public decimal? Rent { get; set; }

    [DataMember(Order = 2)]
    public decimal? Saving { get; set; }

    [DataMember(Order = 3)]
    public decimal? Insurance { get; set; }

    [DataMember(Order = 4)]
    public decimal? Other { get; set; }
}
