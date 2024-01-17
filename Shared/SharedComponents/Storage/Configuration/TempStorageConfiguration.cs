namespace SharedComponents.Storage.Configuration;

public sealed class TempStorageConfiguration
{
    /// <summary>
    /// Maximální délka původního názvu souboru.
    /// </summary>
    public int MaxFileNameSize { get; set; } = 64;

    /// <summary>
    /// Pokud je nastaveno na TRUE, budou se kontrolovat koncovky souborů z AllowedFileExtensions.
    /// </summary>
    public bool UseAllowedFileExtensions { get; set; }

    /// <summary>
    /// Povolené koncovky souborů. Pokud je null, použije se výchozí nastavení.
    /// </summary>
    /// <example>.pdf</example>
    public string[]? AllowedFileExtensions { get; set; } = null!;

    /// <summary>
    /// Connection string na databázi, kam se budou ukládat metadata temp souborů. Pokud je null, bere se default connection string aplikace.
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Nastaveni fyzickeho uloziste souboru
    /// </summary>
    public StorageClientConfiguration StorageClient { get; set; } = null!;
}
