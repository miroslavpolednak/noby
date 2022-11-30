using CIS.Foms.Enums;

namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Výchozí implementace IExternalServiceConfiguration.
/// </summary>
public class ExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration<TClient>
    where TClient : class, IExternalServiceClient
{
    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
    /// <remarks>Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.</remarks>
    public bool LogPayloads { get; set; } = true;

    /// <summary>
    /// Default request timeout in seconds
    /// </summary>
    /// <remarks>Default is set to 10 seconds</remarks>
    public int? RequestTimeout { get; set; } = 10;

    /// <summary>
    /// Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.
    /// </summary>
    public string ServiceUrl { get; set; } = "";

    /// <summary>
    /// If True, then library will try to obtain all needed service URL's from ServiceDiscovery.
    /// </summary>
    /// <remarks>Default is set to True</remarks>
    public bool UseServiceDiscovery { get; set; } = true;

    /// <summary>
    /// Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.
    /// </summary>
    public bool IgnoreServerCertificateErrors { get; set; } = true;

    /// <summary>
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
