namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class CalculateResponse
{
    [DataMember(Order = 1)]
    public long? InstallmentLimit { get; set; }

    [DataMember(Order = 2)]
    public long? MaxAmount { get; set; }

    [DataMember(Order = 3)]
    public long? RemainsLivingAnnuity { get; set; }

    [DataMember(Order = 4)]
    public long? RemainsLivingInst { get; set; }

    [DataMember(Order = 5)]
    public int WorthinessResult { get; set; }

    [DataMember(Order = 6)]
    public ResultReason? ResultReason { get; set; }
}
