namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class LoanApplicationProduct
{
    [DataMember(Order = 1)]
    public int Product { get; set; }

    [DataMember(Order = 2)]
    public int Maturity { get; set; }

    [DataMember(Order = 3)]
    public decimal InterestRate { get; set; }

    [DataMember(Order = 4)]
    public int AmountRequired { get; set; }

    [DataMember(Order = 5)]
    public int Annuity { get; set; }

    [DataMember(Order = 6)]
    public int FixationPeriod { get; set; }
}
