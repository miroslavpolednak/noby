using CIS.Foms.Enums;

namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Základní konfigurace externí služby (služby třetí strany).
/// </summary>
public interface IExternalServiceConfiguration
{
    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
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
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    ServiceImplementationTypes ImplementationType { get; set; }

    /// <summary>
    /// Pokud je true, naplni headers requestu udaji vyzadovanymi KB.
    /// </summary>
    /// <remarks>Budeme potrebovat resit X-KB-Caller-System-Identity ?</remarks>
    bool PropagateKbHeaders { get; set; }
}

public interface IExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration
    where TClient : class
{ }
