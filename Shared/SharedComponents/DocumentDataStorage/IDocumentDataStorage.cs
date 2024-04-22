namespace SharedComponents.DocumentDataStorage;

public interface IDocumentDataStorage
{
    /// <summary>
    /// Deserializace stringu v databázi na požadovanou instanci dat
    /// </summary>
    TData? Deserialize<TData>(ReadOnlySpan<char> data)
        where TData : class, IDocumentData;

    /// <summary>
    /// Serializace instance dat na string do databáze
    /// </summary>
    string Serialize<TData>(TData? data)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle jejího primárního klíče. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="documentDataStorageId">ID instance dat</param>
    Task<DocumentDataItem<TData, TId>?> FirstOrDefault<TData, TId>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle její entity. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<DocumentDataItem<TData, TId>?> FirstOrDefaultByEntityId<TData, TId>(TId entityId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle její entity. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<DocumentDataItem<TData, TId>?> FirstOrDefaultByEntityId<TData, TId>(TId entityId, string tableName, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entity.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData, TId>>> GetList<TData, TId>(TId entityId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entit.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entit pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData, TId>>> GetList<TData, TId>(TId[] entityIds, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Založí novou instanci dat pro danou entitu.
    /// </summary>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="data">Instance dat</param>
    Task<int> Add<TId, TData>(TId entityId, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Založí novou instanci dat pro danou entitu.
    /// </summary>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <param name="data">Instance dat</param>
    Task<int> Add<TId, TData>(TId entityId, string tableName, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle daného primární klíče documentDataStorageId.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="documentDataStorageId">ID instance dat</param>
    /// <param name="data">Instance dat</param>
    Task Update<TData>(int documentDataStorageId, TData data)
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle ID entity pro která byla data uložena.
    /// </summary>
    /// <remarks>
    /// Pokud je pro dané ID entity uloženo více instancí dat, updatuje všechny instance!
    /// </remarks>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="data">Instance dat</param>
    Task UpdateByEntityId<TId, TData>(TId entityId, TData data)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle ID entity pro která byla data uložena.
    /// </summary>
    /// <remarks>
    /// Pokud je pro dané ID entity uloženo více instancí dat, updatuje všechny instance!
    /// </remarks>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <param name="data">Instance dat</param>
    Task UpdateByEntityId<TId, TData>(TId entityId, string tableName, TData data)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle ID entity pro která byla data uložena. Pokud daný záznam neexistuje, vloží ho.
    /// </summary>
    /// <remarks>
    /// Pokud je pro dané ID entity uloženo více instancí dat, updatuje všechny instance!
    /// </remarks>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="data">Instance dat</param>
    Task<int> AddOrUpdateByEntityId<TId, TData>(TId entityId, TData data, CancellationToken cancellationToken)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle ID entity pro která byla data uložena. Pokud daný záznam neexistuje, vloží ho.
    /// </summary>
    /// <remarks>
    /// Pokud je pro dané ID entity uloženo více instancí dat, updatuje všechny instance!
    /// </remarks>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <param name="data">Instance dat</param>
    Task<int> AddOrUpdateByEntityId<TId, TData>(TId entityId, string tableName, TData data, CancellationToken cancellationToken)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže instanci dat v tabulce dané entity dle primárního klíče documentDataStorageId.
    /// </summary>
    /// <param name="documentDataStorageId">ID instance dat</param>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    Task<int> Delete<TData>(int documentDataStorageId)
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže všechny instance dat uložená v tabulce dané entity dle ID entity.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    Task<int> DeleteByEntityId<TId, TData>(TId entityId)
        where TId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže všechny instance dat uložená v tabulce dané entity dle ID entity.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <typeparam name="TId">Generické ID entity</typeparam>
    Task<int> DeleteByEntityId<TId>(TId entityId, string tableName) 
        where TId : IConvertible;
}
