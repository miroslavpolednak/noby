﻿using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SharedComponents.DocumentDataStorage;

internal sealed class DocumentDataStorageProvider(
    CIS.Core.Security.ICurrentUserAccessor _currentUserAccessor,
    TimeProvider _dateTime,
    IConnectionProvider<Database.IDocumentDataStorageConnection> _connectionProvider)
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

    public async Task<DocumentDataItem<TData, TId>?> FirstOrDefault<TData, TId>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem<TId>>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataStorageId=@documentDataStorageId",
            new { documentDataStorageId }, 
            cancellationToken);

        return entity is null ? null : getInner<TData, TId>(entity);
    }

    public async Task<DocumentDataItem<TData, TId>?> FirstOrDefaultByEntityId<TData, TId>(TId entityId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem<TId>>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId=@entityId",
            new { entityId },
            cancellationToken);

        return entity is null ? null : getInner<TData, TId>(entity);
    }

    public async Task<DocumentDataItem<TData, TId>?> FirstOrDefaultByEntityId<TData, TId>(TId entityId, string tableName, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var getSql = $"""
                      SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data
                      FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                      WHERE DocumentDataEntityId=@entityId
                      """;

        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem<TId>>(getSql, new { entityId }, cancellationToken);

        return entity is null ? null : getInner<TData, TId>(entity);
    }

    public async Task<List<DocumentDataItem<TData, TId>>> GetList<TData, TId>(TId entityId, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
        => await GetList<TData, TId>([ entityId ], cancellationToken);

    public async Task<List<DocumentDataItem<TData, TId>>> GetList<TData, TId>(TId[] entityIds, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var entities = await _connectionProvider.ExecuteDapperRawSqlToListAsync<Database.DocumentDataStorageItem<TId>>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId IN @entityIds", 
            new { entityIds }, 
            cancellationToken);

        return entities.Select(t => getInner<TData, TId>(t)).ToList();
    }

    public Task<int> Add<TId, TData>(TId entityId, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return Add<TId, TData>(entityId, getEntityType<TData>(), data, cancellationToken);
    }

    public Task<int> Add<TId, TData>(TId entityId, string tableName, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var (insertSql, varsToInsert) = getVarsToInsert(entityId, tableName, data);
        return _connectionProvider.ExecuteDapperFirstOrDefaultAsync<int>(insertSql, varsToInsert, cancellationToken);
    }

    public Task<int> Add<TId, TData>(IDbConnection connection, IDbTransaction transaction, TId entityId, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return Add<TId, TData>(connection, transaction, entityId, getEntityType<TData>(), data, cancellationToken);
    }

    public Task<int> Add<TId, TData>(IDbConnection connection, IDbTransaction transaction, TId entityId, string tableName, TData data, CancellationToken cancellationToken = default)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var (insertSql, varsToInsert) = getVarsToInsert(entityId, tableName, data);
        return connection.QueryFirstOrDefaultAsync<int>(insertSql, varsToInsert, transaction);
    }

    private (string InsertSql, object VarsToInsert) getVarsToInsert<TId, TData>(in TId entityId, in string tableName, TData data) 
        where TId : IConvertible
        where TData : class, IDocumentData
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
            CreatedTime = _dateTime.GetLocalNow().DateTime
        };

        return (insertSql, varsToInsert);
    }

    public Task Update<TData>(int documentDataStorageId, TData data) 
        where TData : class, IDocumentData
    {
        return Update(documentDataStorageId, getEntityType<TData>(), data);
    }

    public async Task Update<TData>(int documentDataStorageId, string tableName, TData data)
        where TData : class, IDocumentData
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

    public Task UpdateByEntityId<TId, TData>(TId entityId, TData data)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return UpdateByEntityId(entityId, getEntityType<TData>(), data);
    }

    public async Task UpdateByEntityId<TId, TData>(TId entityId, string tableName, TData data)
        where TId : IConvertible
        where TData : class, IDocumentData
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

    public Task<int> AddOrUpdateByEntityId<TId, TData>(TId entityId, TData data, CancellationToken cancellationToken)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return AddOrUpdateByEntityId<TId, TData>(entityId, getEntityType<TData>(), data, cancellationToken);
    }

    public async Task<int> AddOrUpdateByEntityId<TId, TData>(TId entityId, string tableName, TData data, CancellationToken cancellationToken) 
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        var existingIdSql = $"""
                             SELECT TOP 1 DocumentDataStorageId FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName}
                             WHERE DocumentDataEntityId=@entityId
                             """;

        var existingId = await _connectionProvider.ExecuteDapperScalarAsync<int?>(existingIdSql, new { entityId }, cancellationToken);

        if (existingId.HasValue)
        {
            await Update(existingId.Value, tableName, data);
            return existingId.Value;
        }
        else
        {
            return await Add(entityId, tableName, data, cancellationToken);
        }
    }

    public async Task<int> Delete<TData>(int documentDataStorageId)
        where TData : class, IDocumentData
    {
        await using var connection = _connectionProvider.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync($"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataStorageId = @documentDataStorageId", new { documentDataStorageId });
    }

    public Task<int> DeleteByEntityId<TId, TData>(TId entityId)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return DeleteByEntityId(entityId, getEntityType<TData>());
    }

    public async Task<int> DeleteByEntityId<TId>(TId entityId, string tableName) 
        where TId : IConvertible
    {
        await using var connection = _connectionProvider.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync($"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{tableName} WHERE DocumentDataEntityId = @entityId", new { entityId = entityId.ToString(CultureInfo.InvariantCulture) });
    }

    private DocumentDataItem<TData, TId> getInner<TData, TId>(Database.DocumentDataStorageItem<TId> entity)
        where TId : IConvertible
        where TData : class, IDocumentData
    {
        return new DocumentDataItem<TData, TId>(entity.DocumentDataStorageId, entity.DocumentDataVersion, Deserialize<TData>(entity.Data), entity.DocumentDataEntityId);
    }

    private static string getEntityType<TData>()
        where TData : class, IDocumentData
    {
        return typeof(TData).Name;
    }

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}
