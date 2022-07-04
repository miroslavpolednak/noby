namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class LoanAgreement
{
    /// <summary>
    /// Identifikátor obchodního případu.
    /// </summary>
    [DataMember(Order = 1)]
    public string DistributionChannel { get; set; }

    /// <summary>
    /// Identifikátor obchodního případu.
    /// </summary>
    [DataMember(Order = 2)]
    public string SignatureType { get; set; }
}
