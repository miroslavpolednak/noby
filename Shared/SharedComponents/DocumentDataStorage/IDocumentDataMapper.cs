namespace SharedComponents.DocumentDataStorage;

/// <summary>
/// Helper pro mapování entit mezi modelem uloženým v databázi a požadovaným modelem na response / requestu služby
/// </summary>
/// <remarks>
/// Podporuje také DI.
/// </remarks>
/// <typeparam name="TData">Kontrakt entity v databázi</typeparam>
/// <typeparam name="TSource">Objekt na request / response služby</typeparam>
public interface IDocumentDataMapper<TData, TSource> 
    where TSource : class
    where TData : class, IDocumentData
{
    /// <summary>
    /// Mapování na entitu databáze
    /// </summary>
    TData MapToDocumentData(TSource data);

    /// <summary>
    /// Mapování z entity databáze
    /// </summary>
    TSource MapFromDocumentData(TData data);
}
