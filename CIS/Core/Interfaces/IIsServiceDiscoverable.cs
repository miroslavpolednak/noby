namespace CIS.Core;

/// <summary>
/// Rozhraní reprezentující konfigurační objekt, který má možnost pomocí ServiceDiscovery zjistit adresu vlastní služby.
/// </summary>
/// <remarks>
/// Pokud je v projektu přidána reference na ServiceDiscovery (builder.AddCisServiceDiscovery a app.UseServiceDiscovery) a UseServiceDiscovery=true, pokusí se ServiceDiscovery Client načíst URL služby ServiceName.
/// </remarks>
public interface IIsServiceDiscoverable
{
    /// <summary>
    /// Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.
    /// </summary>
    Uri? ServiceUrl { get; set; }

    /// <summary>
    /// If True, then library will try to obtain all needed service URL's from ServiceDiscovery.
    /// </summary>
    /// <remarks>Default is set to True</remarks>
    bool UseServiceDiscovery { get; }

    /// <summary>
    /// Nazev sluzby v ServiceDiscovery
    /// </summary>
    /// <remarks>Je povinné pokud se má dočítat URL služby ze ServiceDiscovery.</remarks>
    string? ServiceName { get; }

    /// <summary>
    /// Typ služby: 1=gRPC, 2=REST, 3=proprietary
    /// </summary>
    int ServiceType { get; }
}
