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
    public Uri? ServiceUrl { get; init; }

    public GrpcServiceUriSettingsDirect(in string? serviceUrl)
    {
        if (string.IsNullOrEmpty(serviceUrl))
            throw new ArgumentNullException(nameof(serviceUrl), $"Service URL in GrpcServiceUriSettingsDirect for {typeof(TService)} is empty or null");

        if (Uri.TryCreate(serviceUrl, UriKind.Absolute, out Uri? serviceUri))
        {
            ServiceUrl = serviceUri!;
        }
        else
        {
            throw new ArgumentNullException(nameof(serviceUrl), $"Service URL in GrpcServiceUriSettingsDirect for {typeof(TService)} is empty or null");
        }
    }
}
