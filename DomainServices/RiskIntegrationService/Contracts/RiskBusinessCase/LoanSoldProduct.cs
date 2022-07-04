namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class LoanSoldProduct
{
    /// <summary>
    /// Identifikátor obchodního případu.
    /// </summary>
    [DataMember(Order = 1)]
    public CompanyId Id { get; set; }
}
