namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class ExpensesSummary
{
    [DataMember(Order = 1)]
    public double? Rent { get; set; }

    [DataMember(Order = 2)]
    public double? Saving { get; set; }

    [DataMember(Order = 3)]
    public double? Insurance { get; set; }

    [DataMember(Order = 4)]
    public double? Other { get; set; }
}
