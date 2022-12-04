namespace CIS.Infrastructure.gRPC.Configuration;

/// <summary>
/// Implementace bez napojení na ServiceDiscovery.
/// </summary>
public sealed class GrpcServiceUriSettingsDirect<TService>
    : IGrpcServiceUriSettings<TService>
    where TService : class
{
    /// <summary>
    /// Adresa služby.
    /// </summary>
    public Uri? ServiceUrl { get; set; }

    public GrpcServiceUriSettingsDirect(in string? serviceUrl)
    {
        if (string.IsNullOrEmpty(serviceUrl))
            throw new Core.Exceptions.CisArgumentNullException(12, "Service URL is empty or null", nameof(serviceUrl));

        ServiceUrl = new Uri(serviceUrl) ?? throw new Core.Exceptions.CisArgumentNullException(12, "Service URL is empty or null", nameof(serviceUrl));
    }
}
