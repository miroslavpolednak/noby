namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class ResultReason
{
    [DataMember(Order = 1)]
    public string? Code { get; set; }

    [DataMember(Order = 2)]
    public string? Description { get; set; }
}
