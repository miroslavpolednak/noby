using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SharedComponents.DocumentDataStorage;

internal sealed class DocumentDataStorageProvider
    : IDocumentDataStorage
{
    public TData? Deserialize<TData>(ReadOnlySpan<char> data)
        where TData : class, IDocumentData
    {
        return JsonSerializer.Deserialize<TData>(data);
    }

    public string Serialize<TData>(TData? data)
        where TData : class, IDocumentData
    {
        return JsonSerializer.Serialize(data, _jsonSerializerOptions);
    }

    public string Serialize(IDocumentData? data) => JsonSerializer.Serialize(data, _jsonSerializerOptions);

    public async Task<DocumentDataItem<TData>?> FirstOrDefault<TData>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataStorageId=@documentDataStorageId",
            new { documentDataStorageId  }, 
            cancellationToken);

        return entity is null ? null : getInner<TData>(entity);
    }

    public async Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await FirstOrDefaultByEntityId<TData>(entityId.ToString(CultureInfo.InvariantCulture), cancellationToken);

    public async Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(long entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await FirstOrDefaultByEntityId<TData>(entityId.ToString(CultureInfo.InvariantCulture), cancellationToken);

    public async Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(string entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId=@entityId",
            new { entityId },
            cancellationToken);

        return entity is null ? null : getInner<TData>(entity);
    }

    public async Task<DocumentDataItem<TData>?> FirstOrDefaultByEntityId<TData>(int entityId, string tableName, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var getSql = $"""
                      SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data
                      FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                      WHERE DocumentDataEntityId=@entityId
                      """;

        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem>(getSql, new { entityId }, cancellationToken);

        return entity is null ? null : getInner<TData>(entity);
    }

    public async Task<List<DocumentDataItem<TData>>> GetList<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await GetList<TData>(new[] { entityId.ToString(CultureInfo.InvariantCulture) }, cancellationToken);

    public async Task<List<DocumentDataItem<TData>>> GetList<TData>(int[] entityIds, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await GetList<TData>(entityIds.Select(t => t.ToString(CultureInfo.InvariantCulture)).ToArray(), cancellationToken);

    public async Task<List<DocumentDataItem<TData>>> GetList<TData>(string entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await GetList<TData>(new[] { entityId }, cancellationToken);

    public async Task<List<DocumentDataItem<TData>>> GetList<TData>(string[] entityIds, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entities = await _connectionProvider.ExecuteDapperRawSqlToListAsync<Database.DocumentDataStorageItem>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId IN @entityIds", 
            new { entityIds }, 
            cancellationToken);

        return entities.Select(t => getInner<TData>(t)).ToList();
    }

    public Task<int> Add<TEntityId, TData>(TEntityId entityId, TData data, CancellationToken cancellationToken = default)
        where TEntityId : IConvertible
        where TData : class, IDocumentData
    {
        return Add<TEntityId, TData>(entityId, getEntityType<TData>(), data, cancellationToken);
    }

    public Task<int> Add<TEntityId, TData>(TEntityId entityId, string tableName, TData data, CancellationToken cancellationToken = default) 
        where TEntityId : IConvertible
        where TData : IDocumentData
    {
        var insertSql = $"""
                         INSERT INTO {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                         (DocumentDataEntityId, DocumentDataVersion, Data, CreatedUserId, CreatedTime)
                         OUTPUT inserted.DocumentDataStorageId
                         VALUES (@DocumentDataEntityId, @DocumentDataVersion, @Data, @CreatedUserId, @CreatedTime)
                         """;

        var varsToInsert = new
        {
            DocumentDataEntityId = entityId.ToString(CultureInfo.InvariantCulture),
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            CreatedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0,
            CreatedTime = _dateTime.Now
        };

        return _connectionProvider.ExecuteDapperFirstOrDefaultAsync<int>(insertSql, varsToInsert, cancellationToken);
    }

    public Task Update<TData>(int documentDataStorageId, TData data) where TData : class, IDocumentData
    {
        return Update(documentDataStorageId, getEntityType<TData>(), data);
    }

    public async Task Update<TData>(int documentDataStorageId, string tableName, TData data)
        where TData : IDocumentData
    {
        var varsToUpdate = new
        {
            DocumentDataStorageId = documentDataStorageId,
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            ModifiedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0
        };

        var updateSql = $"""
                         UPDATE {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                         SET Data=@Data, DocumentDataVersion=@DocumentDataVersion, ModifiedUserId=@ModifiedUserId
                         WHERE DocumentDataStorageId=@DocumentDataStorageId
                         """;

        await using var connection = _connectionProvider.Create();

        await connection.OpenAsync();
        await connection.ExecuteAsync(updateSql, varsToUpdate);
    }

    public Task UpdateByEntityId<TEntityId, TData>(TEntityId entityId, TData data)
        where TEntityId : IConvertible
        where TData : class, IDocumentData
    {
        return UpdateByEntityId(entityId, getEntityType<TData>(), data);
    }

    public async Task UpdateByEntityId<TEntityId, TData>(TEntityId entityId, string tableName, TData data)
        where TEntityId : IConvertible
        where TData : IDocumentData
    {
        var varsToUpdate = new
        {
            DocumentDataEntityId = entityId,
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            ModifiedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0
        };

        var updateSql = $"""
                         UPDATE {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                         SET Data=@Data, DocumentDataVersion=@DocumentDataVersion, ModifiedUserId=@ModifiedUserId
                         WHERE DocumentDataEntityId=@DocumentDataEntityId
                         """;

        await using var connection = _connectionProvider.Create();
        await connection.OpenAsync();

        await connection.ExecuteAsync(updateSql, varsToUpdate);
    }

    public Task AddOrUpdateByEntityId<TEntityId, TData>(TEntityId entityId, TData data, CancellationToken cancellationToken)
        where TEntityId : IConvertible
        where TData : class, IDocumentData
    {
        return AddOrUpdateByEntityId<TEntityId, TData>(entityId, getEntityType<TData>(), data, cancellationToken);
    }

    public async Task AddOrUpdateByEntityId<TEntityId, TData>(TEntityId entityId, string tableName, TData data, CancellationToken cancellationToken) 
        where TEntityId : IConvertible
        where TData : IDocumentData
    {
        var existingIdSql = $"""
                             SELECT TOP 1 DocumentDataStorageId FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                             WHERE DocumentDataEntityId=@entityId
                             """;

        var existingId = await _connectionProvider.ExecuteDapperScalarAsync<int?>(existingIdSql, new { entityId }, cancellationToken);

        if (existingId.HasValue)
        {
            await Update(existingId.Value, tableName, data);

            return;
        }

        await Add<TEntityId, TData>(entityId, tableName, data, cancellationToken);
    }

    public async Task<int> Delete<TData>(int documentDataStorageId)
        where TData : class, IDocumentData
    {
        await using var connection = _connectionProvider.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync($"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataStorageId = @documentDataStorageId", new { documentDataStorageId });
    }

    public Task<int> DeleteByEntityId<TEntityId, TData>(TEntityId entityId)
        where TEntityId : IConvertible
        where TData : class, IDocumentData
    {
        return DeleteByEntityId(entityId, getEntityType<TData>());
    }

    public async Task<int> DeleteByEntityId<TEntityId>(TEntityId entityId, string tableName) 
        where TEntityId : IConvertible
    {
        await using var connection = _connectionProvider.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync($"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName} WHERE DocumentDataEntityId = @entityId", new { entityId = entityId.ToString(CultureInfo.InvariantCulture) });
    }

    private DocumentDataItem<TData> getInner<TData>(Database.DocumentDataStorageItem entity)
        where TData : class, IDocumentData
    {
        return new DocumentDataItem<TData>(entity.DocumentDataStorageId, entity.DocumentDataVersion, Deserialize<TData>(entity.Data), entity.DocumentDataEntityId);
    }

    private static string getEntityType<TData>()
        where TData : class, IDocumentData
    {
        return typeof(TData).Name;
    }

    private readonly CIS.Core.Security.ICurrentUserAccessor _currentUserAccessor;
    private readonly CIS.Core.IDateTime _dateTime;
    private readonly IConnectionProvider<Database.IDocumentDataStorageConnection> _connectionProvider;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public DocumentDataStorageProvider(
        CIS.Core.Security.ICurrentUserAccessor currentUserAccessor, 
        CIS.Core.IDateTime dateTime, 
        IConnectionProvider<Database.IDocumentDataStorageConnection> connectionProvider)
    {
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
        _connectionProvider = connectionProvider;
    }
}
