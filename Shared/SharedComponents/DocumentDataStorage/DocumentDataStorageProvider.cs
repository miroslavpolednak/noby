using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace SharedComponents.DocumentDataStorage;

internal class DocumentDataStorageProvider
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

    public async Task<(int? Version, TDestination? Data)> GetDataWithMapper<TData, TDestination>(int entityId, CancellationToken cancellationToken = default)
        where TDestination : class
        where TData : class, IDocumentData
    {
        var service = _serviceProvider.GetService<IDocumentDataMapper<TData, TDestination>>();

        if (service is null)
        {
            throw new ArgumentNullException(nameof(TDestination), $"Mapper for IDocumentData<{typeof(TData).Name},{typeof(TDestination).Name}> not found");
        }

        var model = await GetData<TData>(entityId, cancellationToken);
        if (model.Version.HasValue)
        {
            return (model.Version, service!.MapFromDocumentData(model.Data!));
        }
        else
        {
            return (null, default(TDestination));
        }
    }

    public async Task<(int? Version, TData? Data)> GetData<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        string entityType = typeof(TData).Name;

        var loadedEntity = await _dbContext
            .DocumentsData
            .AsNoTracking()
            .Where(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType)
            .Select(t => new { t.Data, t.DocumentDataVersion })
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrEmpty(loadedEntity?.Data))
        {
            return (null, default(TData));
        }
        else
        {
            return (loadedEntity.DocumentDataVersion, Deserialize<TData>(loadedEntity.Data));
        }
    }

    public async Task InsertOrUpdateDataWithMapper<TData, TSource>(TSource mappedEntity, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TSource : class
        where TData : class, IDocumentData
    {
        var service = _serviceProvider.GetService<IDocumentDataMapper<TData, TSource>>();
        
        if (service is null)
        {
            throw new ArgumentNullException(nameof(TSource), $"Mapper for IDocumentData<{typeof(TData).Name},{typeof(TSource).Name}> not found");
        }

        await InsertOrUpdateData(service!.MapToDocumentData(mappedEntity), entityId, removeOtherStoredEntityTypes, cancellationToken);
    }

    public async Task InsertOrUpdateData<TData>(TData data, int entityId, bool removeOtherStoredEntityTypes = false, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        string entityType = typeof(TData).Name;

        // odstranit data ulozena ke stejne entite ale pod jinym Type klicem
        if (removeOtherStoredEntityTypes)
        {
            await _dbContext
                .DocumentsData
                .Where(t => t.DocumentDataEntityId == entityId && t.DocumentDataType != entityType)
                .ExecuteDeleteAsync(cancellationToken);
        }

        var loadedEntity = await _dbContext
            .DocumentsData
            .FirstOrDefaultAsync(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType, cancellationToken);

        if (loadedEntity is null)
        {
            loadedEntity = new Database.DocumentDataStorage
            {
                DocumentDataEntityId = entityId,
                DocumentDataType = entityType
            };
            _dbContext.DocumentsData.Add(loadedEntity);
        }

        loadedEntity.DocumentDataVersion = data.Version;
        loadedEntity.Data = Serialize(data);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(int entityId, CancellationToken cancellationToken = default)
    {
        await _dbContext.DocumentsData
            .Where(t => t.DocumentDataEntityId == entityId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task Delete<TData>(int entityId, CancellationToken cancellationToken = default)
        where TData : class, IDocumentData
    {
        string entityType = typeof(TData).Name;

        await _dbContext.DocumentsData
            .Where(t => t.DocumentDataEntityId == entityId && t.DocumentDataType == entityType)
            .ExecuteDeleteAsync(cancellationToken);
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
