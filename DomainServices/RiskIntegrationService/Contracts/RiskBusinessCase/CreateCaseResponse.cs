namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CreateCaseResponse
{
    /// <summary>
    /// identifikátor obchodního případu v C4M (ResoureIdentifier.Id)
    /// </summary>
    [DataMember(Order = 1)]
    public string RiskBusinessCaseIdMp { get; set; } = default!;
}
