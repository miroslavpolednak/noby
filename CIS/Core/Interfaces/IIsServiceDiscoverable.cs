namespace CIS.Core;

public interface IIsServiceDiscoverable
{
    /// <summary>
    /// Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.
    /// </summary>
    string ServiceUrl { get; set; }

    /// <summary>
    /// If True, then library will try to obtain all needed service URL's from ServiceDiscovery.
    /// </summary>
    /// <remarks>Default is set to True</remarks>
    bool UseServiceDiscovery { get; }

    /// <summary>
    /// Nazev sluzby v ServiceDiscovery
    /// </summary>
    string ServiceName { get; }

    int ServiceType { get; }
}
