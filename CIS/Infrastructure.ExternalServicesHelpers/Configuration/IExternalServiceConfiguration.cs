using CIS.Foms.Enums;

namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Základní konfigurace externí služby (služby třetí strany).
/// </summary>
/// <remarks>Pro registraci HTTP klienta by se vždy měla používat generická verze interface.</remarks>
public interface IExternalServiceConfiguration
{
    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
    /// <remarks>Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.</remarks>
    bool LogPayloads { get; set; }

    /// <summary>
    /// Default request timeout in seconds
    /// </summary>
    /// <remarks>Default is set to 10 seconds</remarks>
    int? RequestTimeout { get; set; }

    /// <summary>
    /// Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.
    /// </summary>
    string ServiceUrl { get; set; }

    /// <summary>
    /// If True, then library will try to obtain all needed service URL's from ServiceDiscovery.
    /// </summary>
    /// <remarks>Default is set to True</remarks>
    bool UseServiceDiscovery { get; set; }

    /// <summary>
    /// Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.
    /// </summary>
    bool IgnoreServerCertificateErrors { get; set; }

    /// <summary>
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    ServiceImplementationTypes ImplementationType { get; set; }
}

/// <summary>
/// Generická verze konfigurace.
/// </summary>
/// <typeparam name="TClient">Typ HTTP klienta</typeparam>
public interface IExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration
    where TClient : class, IExternalServiceClient
{ }
