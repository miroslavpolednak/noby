using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
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
    {
        var entity = await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Database.DocumentDataStorageItem>(
            $"SELECT DocumentDataStorageId, DocumentDataVersion, DocumentDataEntityId, Data FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId=@entityId",
            new { entityId },
            cancellationToken);

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

    public async Task<int> Add<TData>(TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var varsToInsert = new
        {
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            CreatedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0,
            CreatedTime = _dateTime.Now
        };

        return await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<int>(@$"
            INSERT INTO {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} 
            (DocumentDataVersion, Data, CreatedUserId, CreatedTime) 
            OUTPUT inserted.DocumentDataStorageId 
            VALUES 
            (@DocumentDataVersion, @Data, @CreatedUserId, @CreatedTime)",
            varsToInsert,
            cancellationToken);
    }

    public async Task<int> Add<TData>(int entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
        => await Add(entityId.ToString(CultureInfo.InvariantCulture), data, cancellationToken);

    public async Task<int> Add<TData>(string entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var varsToInsert = new
        {
            DocumentDataEntityId = entityId,
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            CreatedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0,
            CreatedTime = _dateTime.Now
        };

        return await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<int>(@$"
            INSERT INTO {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} 
            (DocumentDataEntityId, DocumentDataVersion, Data, CreatedUserId, CreatedTime) 
            OUTPUT inserted.DocumentDataStorageId 
            VALUES 
            (@DocumentDataEntityId, @DocumentDataVersion, @Data, @CreatedUserId, @CreatedTime)", 
            varsToInsert, 
            cancellationToken);
    }

    public async Task Update<TData>(int documentDataStorageId, TData data)
        where TData : class, IDocumentData
    {
        var varsToUpdate = new
        {
            DocumentDataStorageId = documentDataStorageId,
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            ModifiedUserId  = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0
        };

        using (var connection = _connectionProvider.Create())
        {
            connection.Open();
            await connection.ExecuteAsync(
                $"UPDATE {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} SET Data=@Data, DocumentDataVersion=@DocumentDataVersion, ModifiedUserId=@ModifiedUserId WHERE DocumentDataStorageId=@DocumentDataStorageId",
                varsToUpdate);
        }
    }

    public async Task AddOrUpdateByEntityId<TData>(int entityId, TData data, CancellationToken cancellationToken)
        where TData : class, IDocumentData
    {
        var existingId = await _connectionProvider.ExecuteDapperScalarAsync<int?>($"SELECT TOP 1 DocumentDataStorageId FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId=@entityId", new { entityId }, cancellationToken);

        if (!existingId.HasValue)
        {
            await Add(entityId, data, cancellationToken);
        }
        else
        {
            await Update(existingId.Value, data);
        }
    }

    public async Task UpdateByEntityId<TData>(int entityId, TData data)
        where TData : class, IDocumentData
        => await UpdateByEntityId(entityId.ToString(CultureInfo.InvariantCulture), data);

    public async Task UpdateByEntityId<TData>(string entityId, TData data)
        where TData : class, IDocumentData
    {
        var varsToUpdate = new
        {
            DocumentDataEntityId = entityId,
            DocumentDataVersion = data.Version,
            Data = Serialize(data),
            ModifiedUserId = _currentUserAccessor.IsAuthenticated ? _currentUserAccessor.User!.Id : 0
        };

        using (var connection = _connectionProvider.Create())
        {
            connection.Open();
            await connection.ExecuteAsync(
                $"UPDATE {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} SET Data=@Data, DocumentDataVersion=@DocumentDataVersion, ModifiedUserId=@ModifiedUserId WHERE DocumentDataEntityId=@DocumentDataEntityId",
                varsToUpdate);
        }
    }

    public async Task<int> Delete<TData>(int documentDataStorageId)
        where TData : class, IDocumentData
    {
        using (var connection = _connectionProvider.Create())
        {
            connection.Open();
            return await connection.ExecuteAsync(
                $"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataStorageId = @documentDataStorageId", 
                new { documentDataStorageId });
        }
    }

    public async Task<int> DeleteByEntityId<TData>(int entityId)
        where TData : class, IDocumentData
        => await DeleteByEntityId<TData>(entityId.ToString(CultureInfo.InvariantCulture));

    public async Task<int> DeleteByEntityId<TData>(string entityId)
        where TData : class, IDocumentData
    {
        using (var connection = _connectionProvider.Create())
        {
            connection.Open();
            return await connection.ExecuteAsync(
                $"DELETE FROM {DocumentDataStorageConstants.DatabaseSchema}.{getEntityType<TData>()} WHERE DocumentDataEntityId = @entityId", 
                new { entityId });
        }
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

    private static JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Default;

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
