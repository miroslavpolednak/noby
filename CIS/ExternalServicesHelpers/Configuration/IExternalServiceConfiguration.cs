using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public interface IExternalServiceConfiguration
{
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
}
