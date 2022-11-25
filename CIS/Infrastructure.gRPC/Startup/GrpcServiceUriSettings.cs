namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Reprezentuje konfiguraci připojení na doménovou službu.
/// </summary>
/// <remarks>
/// Jedná se o třídu s generickým parametrem, protože mohu mít v projektu napojených více gRPC služeb. Pak je TService typem klienta pro každou službu.
/// </remarks>
public sealed class GrpcServiceUriSettings<TService>
    where TService : class
{
    /// <summary>
    /// Typ klienta (Clients projekt) pro danou konfiguraci.
    /// </summary>
    public Type ServiceType { get; init; }

    /// <summary>
    /// Adresa služby.
    /// </summary>
    public Uri Url { get; init; }

    public GrpcServiceUriSettings(string serviceUrl)
    {
        if (string.IsNullOrEmpty(serviceUrl))
            throw new Core.Exceptions.CisArgumentNullException(12, "Service URL is empty or null", nameof(serviceUrl));

        ServiceType = typeof(TService);
        Url = new Uri(serviceUrl);
    }
}
