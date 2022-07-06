namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class LoanApplicationIncome
{
    [DataMember(Order = 1)]
    public int CategoryMp { get; set; }

    [DataMember(Order = 2)]
    public double Amount { get; set; }
}
