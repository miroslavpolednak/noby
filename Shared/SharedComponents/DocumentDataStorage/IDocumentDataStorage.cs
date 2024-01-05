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
    /// Serializace instance dat na string do databáze
    /// </summary>
    string Serialize(IDocumentData? data);

    /// <summary>
    /// Vrátí nalezenou instanci dat dle jejího primárního klíče. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="documentDataStorageId">ID instance dat</param>
    Task<DocumentDataItem<TData>?> FirstOrDefault<TData>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle její entity. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(string entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle její entity. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí nalezenou instanci dat dle její entity. Pokud instance data neexistuje, vrací NULL.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(long entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entity.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData>>> GetList<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entity.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData>>> GetList<TData>(string entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entit.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entit pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData>>> GetList<TData>(int[] entityIds, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Vrátí instance dat dle ID entit.
    /// </summary>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entit pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    Task<List<DocumentDataItem<TData>>> GetList<TData>(string[] entityIds, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;

    /// <summary>
    /// Založí novou instanci dat pro danou entitu.
    /// </summary>
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="data">Instance dat</param>
    Task<int> Add<TEntityId, TData>(TEntityId entityId, TData data, CancellationToken cancellationToken = default)
        where TEntityId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Založí novou instanci dat pro danou entitu.
    /// </summary>
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <param name="data">Instance dat</param>
    Task<int> Add<TEntityId, TData>(TEntityId entityId, string tableName, TData data, CancellationToken cancellationToken = default)
        where TEntityId : IConvertible
        where TData : IDocumentData;

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
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="data">Instance dat</param>
    Task UpdateByEntityId<TEntityId, TData>(TEntityId entityId, TData data)
        where TEntityId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Updatuje instanci dat v databázi dle ID entity pro která byla data uložena.
    /// </summary>
    /// <remarks>
    /// Pokud je pro dané ID entity uloženo více instancí dat, updatuje všechny instance!
    /// </remarks>
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <param name="data">Instance dat</param>
    Task UpdateByEntityId<TEntityId, TData>(TEntityId entityId, string tableName, TData data)
        where TEntityId : IConvertible
        where TData : IDocumentData;

    Task AddOrUpdateByEntityId<TEntityId, TData>(TEntityId entityId, TData data, CancellationToken cancellationToken)
        where TEntityId : IConvertible
        where TData : class, IDocumentData;

    Task AddOrUpdateByEntityId<TEntityId, TData>(TEntityId entityId, string tableName, TData data, CancellationToken cancellationToken)
        where TEntityId : IConvertible
        where TData : IDocumentData;

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
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    /// <typeparam name="TData">Entita zastupující ukládaná data / název tabulky v databázi</typeparam>
    Task<int> DeleteByEntityId<TEntityId, TData>(TEntityId entityId)
        where TEntityId : IConvertible
        where TData : class, IDocumentData;

    /// <summary>
    /// Smaže všechny instance dat uložená v tabulce dané entity dle ID entity.
    /// </summary>
    /// <param name="entityId">ID entity pro která byla data uložena (např. CustomerOnSAId, IncomeId atd.)</param>
    /// <param name="tableName">Název DDS tabulky</param>
    /// <typeparam name="TEntityId">Generické ID entity</typeparam>
    Task<int> DeleteByEntityId<TEntityId>(TEntityId entityId, string tableName) 
        where TEntityId : IConvertible;

    Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(int entityId, string tableName, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData;
}
