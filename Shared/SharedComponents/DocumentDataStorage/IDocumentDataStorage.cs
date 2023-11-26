namespace SharedComponents.DocumentDataStorage;

public interface IDocumentDataStorage
{
    /// <summary>
    /// Deserializace entity v databázi na požadový model
    /// </summary>
    public static virtual TData? Deserialize<TData>(ReadOnlySpan<char> data)
        where TData : class, IDocumentData
        => throw new NotImplementedException();

    /// <summary>
    /// Serializace modelu na string uložitelný do databáze
    /// </summary>
    public static virtual string Serialize<TData>(TData? data)
        where TData : class, IDocumentData
        => throw new NotImplementedException();

    Task<DocumentDataItem<TData>?> FirstOrDefault<TData>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task<List<DocumentDataItem<TData>>> GetList<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task<int> Add<TData>(int entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task Update<TData>(int documentDataStorageId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task UpdateByEntityId<TData>(int entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže data uložená pro danou entitu v databázi.
    /// </summary>
    /// <param name="documentDataStorageId">ID záznamu</param>
    Task<int> Delete(int documentDataStorageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smaže data uložená v databázi pro kombinaci entity a typu kontraktu.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <typeparam name="TData">Typ kontraktu, který má být smazán</typeparam>
    Task<int> DeleteByEntityId<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;
}
