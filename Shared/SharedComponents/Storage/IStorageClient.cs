namespace SharedComponents.Storage;

public interface IStorageClient<TStorage>
{
    /// <summary>
    /// Uložení souboru do úložiště.
    /// </summary>
    /// <param name="fileName">Název souboru</param>
    /// <param name="folderOrContainer">Adresář nebo kontajner do kterého bude souboru uložený. Může být NULL (soubor bude uložený do rootu BasePath/Bucket/Container).</param>
    Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uložení souboru do úložiště.
    /// </summary>
    /// <param name="fileName">Název souboru</param>
    /// <param name="folderOrContainer">Adresář nebo kontajner do kterého bude souboru uložený. Může být NULL (soubor bude uložený do rootu BasePath/Bucket/Container).</param>
    Task SaveFile(Stream data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Načtení souboru z úložiště.
    /// </summary>
    /// <param name="fileName">Název souboru</param>
    /// <param name="folderOrContainer">Adresář nebo kontajner ve kterém je souboru uložený. Může být NULL.</param>
    Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smazání souboru z úložiště.
    /// </summary>
    /// <param name="fileName">Název souboru</param>
    /// <param name="folderOrContainer">Adresář nebo kontajner ve kterém je souboru uložený. Může být NULL.</param>
    Task DeleteFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);
}
