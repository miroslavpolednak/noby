using System.Text.Json.Serialization;

namespace SharedComponents.Storage;

/// <summary>
/// Instance souboru v temp storage
/// </summary>
public struct TempStorageItem
{
    /// <summary>
    /// TraceId requestu v rámci kterého proběhlo uložení souboru
    /// </summary>
    [JsonIgnore]
    public string? TraceId { get; set; }

    /// <summary>
    /// ID souboru
    /// </summary>
    public Guid TempStorageItemId { get; set; }

    /// <summary>
    /// Původní název souboru
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Mime typ souboru
    /// </summary>
    public string MimeType { get; set; }

    /// <summary>
    /// ID objektu, ke kterému soubor patří
    /// </summary>
    public long? ObjectId { get; set; }

    /// <summary>
    /// Typ objektu, ke kterému soubor patří
    /// </summary>
    public string? ObjectType { get; set; }

    /// <summary>
    /// ID session, ke které soubor patří
    /// </summary>
    public Guid? SessionId { get; set; }
}
