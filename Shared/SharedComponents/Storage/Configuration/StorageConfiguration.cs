namespace SharedComponents.Storage.Configuration;

public sealed class StorageConfiguration
{
    /// <summary>
    /// Nastavení temp úložiště souborů
    /// </summary>
    public TempStorageConfiguration? TempStorage { get; set; }

    /// <summary>
    /// Seznam konfigurací úložišť, které používá aplikace
    /// </summary>
    /// <remarks>
    /// Dictionary Key = název storage clienta (tj. název marker interface).
    /// </remarks>
    public Dictionary<string, StorageClientConfiguration>? StorageClients { get; set; }
}
