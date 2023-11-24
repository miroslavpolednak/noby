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

    /// <summary>
    /// Načte data z databáze a vytvoří z nich požadovanou entitu. Tuto entitu pak pomocí mapperu zkonvertuje do nového objektu - např. response objektu služby.
    /// </summary>
    /// <remarks>
    /// Aby mapování fungovalo, musí být vytvořen mapper IDocumentDataMapper pro danou kombinaci TData a TDestination.
    /// </remarks>
    /// <typeparam name="TData">Kontrakt pro deserializaci</typeparam>
    /// <typeparam name="TDestination">Cílový objekt do kterého mají být data z databáze přemapována</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <returns>
    /// Pokud byla požadovaná data nalezena, vrátí jejich deserializovanou reprezentaci a číslo verze kontraktu ve kterém jsou data uložena.
    /// Pokud v databázi požadovaná data neexistují, vrátí Version=null.
    /// </returns>
    Task<(int? Version, TDestination? Data)> GetDataWithMapper<TData, TDestination>(int entityId, CancellationToken cancellationToken = default)
        where TDestination : class
        where TData : class, IDocumentData;

    /// <summary>
    /// Načte data z databáze a vytvoří z nich požadovanou entitu
    /// </summary>
    /// <typeparam name="TData">Kontrakt pro deserializaci</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <returns>
    /// Pokud byla požadovaná data nalezena, vrátí jejich deserializovanou reprezentaci a číslo verze kontraktu ve kterém jsou data uložena.
    /// Pokud v databázi požadovaná data neexistují, vrátí Version=null.
    /// </returns>
    Task<(int? Version, TData? Data)> GetData<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task InsertOrUpdateData<TData>(TData data, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    Task InsertOrUpdateDataWithMapper<TData, TSource>(TSource mappedEntity, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TSource : class
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže data uložená pro danou entitu v databázi.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task Delete(int entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smaže data uložená v databázi pro kombinaci entity a typu kontraktu.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <typeparam name="TData">Typ kontraktu, který má být smazán</typeparam>
    Task Delete<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;
}
