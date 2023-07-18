namespace ExternalServices.ESignatures.Configuration;

internal sealed class ESignaturesConfiguration<TClient>
    : CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
{
    /// <summary>
    /// Klient ePodpisu: kb, mpss
    /// </summary>
    public string Tenant { get; set; } = "kb";
}
