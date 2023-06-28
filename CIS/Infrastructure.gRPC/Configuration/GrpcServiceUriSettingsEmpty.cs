namespace CIS.Infrastructure.gRPC.Configuration;

public sealed class GrpcServiceUriSettingsEmpty<TService>
    : IGrpcServiceUriSettings<TService>
    where TService : class
{
    /// <summary>
    /// Adresa služby.
    /// </summary>
    public Uri? ServiceUrl { get; init; }

    public GrpcServiceUriSettingsEmpty()
    {
        ServiceUrl = new Uri(GrpcServiceUriSettingsConstants.EmptyUriAddress);
    }
}
