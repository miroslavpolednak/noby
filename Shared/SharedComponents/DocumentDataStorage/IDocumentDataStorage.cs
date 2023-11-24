namespace SharedComponents.DocumentDataStorage;

public interface IDocumentDataStorage
{
    public static virtual TData? Deserialize<TData>(ReadOnlySpan<char> data)
        where TData : class, IDocumentData
        => throw new NotImplementedException();

    public static virtual string Serialize<TData>(TData? data)
        where TData : class, IDocumentData
        => throw new NotImplementedException();

    Task<(int? Version, TDestination? Data)> GetDataWithMapper<TData, TDestination>(int entityId, CancellationToken cancellationToken = default)
        where TDestination : class
        where TData : class, IDocumentData;

    Task<(int? Version, TData? Data)> GetData<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task InsertOrUpdateData<TData>(TData data, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task InsertOrUpdateDataWithMapper<TData, TSource>(TSource mappedEntity, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TSource : class
        where TData : class, IDocumentData;

    Task Delete(int entityId, CancellationToken cancellationToken = default);

    Task Delete<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;
}
