namespace CIS.Infrastructure.gRPC.Configuration;

/// <summary>
/// Implementace podporující ServiceDiscovery.
/// </summary>
public sealed class GrpcServiceUriSettingsServiceDiscovery<TService>
    : IGrpcServiceUriSettings<TService>, Core.IIsServiceDiscoverable
    where TService : class
{
    public bool UseServiceDiscovery { get; } = true;

    public string? ServiceName { get; init; }

    /// <summary>
    /// Always gRPC (=1)
    /// </summary>
    public int ServiceType { get; } = 1;

    /// <summary>
    /// Adresa služby.
    /// </summary>
    public Uri? ServiceUrl { get; set; }

    public GrpcServiceUriSettingsServiceDiscovery(in string? serviceName)
    {
        if (string.IsNullOrEmpty(serviceName))
            throw new ArgumentNullException(nameof(serviceName), $"ServiceName in GrpcServiceUriSettingsServiceDiscovery for {typeof(TService)} is empty or null");

        ServiceName = serviceName;
    }
}
