namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CollateralAgreement
{
    // 15.03. MR : zmena typu z ResourceIdentifier na String
    /// <summary>
    /// Identifikátor obchodního případu.
    /// </summary>
    /// <value>Identifikátor obchodního případu.</value>
    [DataMember(Order = 1)]
    public string Id { get; set; }
}
