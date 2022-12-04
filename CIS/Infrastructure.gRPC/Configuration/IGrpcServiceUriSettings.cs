namespace CIS.Infrastructure.gRPC.Configuration;

/// <summary>
/// Reprezentuje konfiguraci připojení na doménovou službu.
/// </summary>
/// <remarks>
/// Jedná se o třídu s generickým parametrem, protože mohu mít v projektu napojených více gRPC služeb. Pak je TService typem klienta pro každou službu.
/// </remarks>
public interface IGrpcServiceUriSettings<TService>
    where TService : class
{
    /// <summary>
    /// Adresa služby.
    /// </summary>
    Uri? ServiceUrl { get; set; }
}
