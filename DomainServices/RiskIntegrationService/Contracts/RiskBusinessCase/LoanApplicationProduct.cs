namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class LoanApplicationProduct
{
    /// <summary>
    /// kód produktového shluku (shluk jednoho produktu).
    /// </summary>
    /// <value>kód produktového shluku (shluk jednoho produktu).</value>
    [DataMember(Order = 1)]
    public int Product { get; set; }
}
