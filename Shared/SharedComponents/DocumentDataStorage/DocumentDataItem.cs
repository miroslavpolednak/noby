namespace SharedComponents.DocumentDataStorage;

public sealed record DocumentDataItem<TData>(int DocumentDataStorageId, int Version, int EntityId, TData? Data)
    where TData : class, IDocumentData
{
}
