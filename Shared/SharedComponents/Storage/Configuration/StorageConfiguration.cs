namespace SharedComponents.Storage.Configuration;

public sealed class StorageConfiguration
{
    /// <summary>
    /// Nastavení temp úložiště souborů
    /// </summary>
    public TempStorageConfiguration? TempStorage { get; set; }
}
