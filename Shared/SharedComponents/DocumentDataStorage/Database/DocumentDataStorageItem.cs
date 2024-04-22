namespace SharedComponents.DocumentDataStorage.Database;

internal sealed class DocumentDataStorageItem<TId>
    where TId : IConvertible
{
    public int DocumentDataStorageId { get; set; }
    public TId? DocumentDataEntityId { get; set; }
    public int DocumentDataVersion { get; set; }
    public string? Data { get; set; }
}
