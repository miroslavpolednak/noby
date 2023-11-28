namespace SharedComponents.DocumentDataStorage.Database;

internal sealed class DocumentDataStorageItem
{
    public int DocumentDataStorageId { get; set; }
    public int DocumentDataEntityId { get; set; }
    public int DocumentDataVersion { get; set; }
    public string? Data { get; set; }
}
