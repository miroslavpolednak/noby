using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace SharedComponents.DocumentDataStorage;

internal sealed class DocumentDataStorageProvider
    : IDocumentDataStorage
{
    public static TData? Deserialize<TData>(ReadOnlySpan<char> data)
        where TData : class, IDocumentData
    {
        return JsonSerializer.Deserialize<TData>(data);
    }

    public static string Serialize<TData>(TData? data)
        where TData : class, IDocumentData
    {
        return JsonSerializer.Serialize(data, _jsonSerializerOptions);
    }

    public async Task<DocumentDataItem<TData>?> FirstOrDefault<TData>(int documentDataStorageId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entity = await _dbContext
            .DocumentsData
            .AsNoTracking()
            .Where(t => t.DocumentDataStorageId == documentDataStorageId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (entity.DocumentDataType != typeof(TData).Name)
        {
            throw new InvalidOperationException($"DocumentDataStorageId {documentDataStorageId} is not of type {typeof(TData).Name}");
        }

        return getInner<TData>(entity);
    }

    public async Task<List<DocumentDataItem<TData>>> GetList<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        string entityType = typeof(TData).Name;

        var entity = await _dbContext
            .DocumentsData
            .AsNoTracking()
            .Where(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType)
            .ToListAsync(cancellationToken);

        return entity.Select(t =>
            {
                if (t.DocumentDataType != entityType)
                {
                    throw new InvalidOperationException($"DocumentDataStorageId {t.DocumentDataStorageId} is not of type {typeof(TData).Name}");
                }

                return getInner<TData>(t);
            })
            .ToList();
    }

    public async Task<int> Add<TData>(int entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entity = new Database.DocumentDataStorage
        {
            DocumentDataEntityId = entityId,
            DocumentDataType = typeof(TData).Name
        };
        _dbContext.DocumentsData.Add(entity);

        updateEntityFromData(entity, data);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.DocumentDataStorageId;
    }

    public async Task Update<TData>(int documentDataStorageId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entity = await _dbContext
            .DocumentsData
            .FirstAsync(t => t.DocumentDataStorageId == documentDataStorageId, cancellationToken);

        updateEntityFromData(entity, data);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateByEntityId<TData>(int entityId, TData data, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        var entityType = typeof(TData).Name;

        var entity = await _dbContext
            .DocumentsData
            .SingleAsync(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType, cancellationToken);

        updateEntityFromData(entity, data);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Delete(int documentDataStorageId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DocumentsData
            .Where(t => t.DocumentDataStorageId == documentDataStorageId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int> DeleteByEntityId<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        string entityType = typeof(TData).Name;

        return await _dbContext.DocumentsData
            .Where(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static void updateEntityFromData<TData>(Database.DocumentDataStorage entity, TData data)
        where TData : class, IDocumentData
    {
        entity.DocumentDataVersion = data.Version;
        entity.Data = Serialize(data);
    }

    private static DocumentDataItem<TData> getInner<TData>(Database.DocumentDataStorage entity)
        where TData : class, IDocumentData
    {
        return new DocumentDataItem<TData>(entity.DocumentDataStorageId, entity.DocumentDataVersion, entity.DocumentDataEntityId, Deserialize<TData>(entity.Data));
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly Database.DocumentDataDbContext _dbContext;

    private static JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Default;

    public DocumentDataStorageProvider(Database.DocumentDataDbContext dbContext, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _dbContext = dbContext;
    }
}
