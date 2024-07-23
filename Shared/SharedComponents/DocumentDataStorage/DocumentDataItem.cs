namespace SharedComponents.DocumentDataStorage;

public sealed record DocumentDataItem<TData, TId>(int DocumentDataStorageId, int Version, TData? Data, TId? EntityId)
    where TData : class, IDocumentData
{
}
