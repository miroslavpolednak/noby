using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharedComponents.Storage.Database;
using System.Diagnostics;

namespace SharedComponents.Storage;

internal sealed class TempStorage
    : ITempStorage
{
    public async Task<TempStorageItem> Save(
        IFormFile file,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        var fileInstance = new TempStorageItem
        {
            TempStorageItemId = Guid.NewGuid(),
            FileName = file.FileName ?? Guid.NewGuid().ToString(),
            MimeType = file.ContentType ?? "",
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType,
            TraceId = Activity.Current?.Id
        };

        validateFile(fileInstance);

        // zapsat na disk
        await _storageClient.SaveFile(file.OpenReadStream(), fileInstance.TempStorageItemId.ToString(), cancellationToken: cancellationToken);

        // ulozit do DB
        await saveFileToDatabase(fileInstance, cancellationToken);
        
        return fileInstance;
    }

    public async Task<TempStorageItem> Save(
        byte[] fileData,
        string mimeType,
        string? fileName = null,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        var guid = Guid.NewGuid();
        var fileInstance = new TempStorageItem
        {
            TempStorageItemId = guid,
            FileName = fileName ?? guid.ToString(),
            MimeType = mimeType,
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType,
            TraceId = Activity.Current?.Id
        };

        validateFile(fileInstance);

        // zapsat na disk
        await _storageClient.SaveFile(fileData, fileInstance.TempStorageItemId.ToString(), cancellationToken:  cancellationToken);

        // ulozit do DB
        await saveFileToDatabase(fileInstance, cancellationToken);

        return fileInstance;
    }

    public async Task<List<TempStorageItem>> GetSession(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _connectionProvider
            .ExecuteDapperRawSqlToListAsync<TempStorageItem>(
                $"SELECT * FROM {_tableName} WHERE SessionId=@sessionId", 
                new { sessionId }, 
                cancellationToken
            );
    }

    public async Task<List<TempStorageItem>> GetByObjectType(string objectType, long objectId, CancellationToken cancellationToken = default)
    {
        return await _connectionProvider
            .ExecuteDapperRawSqlToListAsync<TempStorageItem>(
                $"SELECT * FROM {_tableName} WHERE ObjectType=@objectType AND ObjectId=@objectId",
                new { objectType, objectId },
                cancellationToken
            );
    }

    public async Task<TempStorageItem> GetMetadata(Guid tempStorageItemId, CancellationToken cancellationToken = default)
    {
        return await _connectionProvider
            .ExecuteDapperFirstOrDefaultAsync<TempStorageItem>(
            $"SELECT * FROM {_tableName} WHERE TempStorageItemId=@tempStorageItemId",
            new { tempStorageItemId },
            cancellationToken);
    }

    public async Task<byte[]> GetContent(Guid tempStorageItemId, CancellationToken cancellationToken = default)
    {
        return await _storageClient.GetFile(tempStorageItemId.ToString(), cancellationToken: cancellationToken);
    }

    public async Task Delete(Guid tempStorageItemId, CancellationToken cancellationToken = default)
    {
        await _connectionProvider
            .ExecuteDapperAsync(
                $"DELETE FROM {_tableName} WHERE TempStorageItemId=@tempStorageItemId", 
                new { tempStorageItemId }, 
                cancellationToken
            );
        
        // v celku nas nezajima, jestli je soubor smazany nebo ne
        try
        {
            await _storageClient.DeleteFile(tempStorageItemId.ToString(), cancellationToken: cancellationToken);
        }
        catch { }
    }

    public async Task Delete(IEnumerable<Guid> tempStorageItemId, CancellationToken cancellationToken = default)
    {
        await _connectionProvider
            .ExecuteDapperAsync(
                $"DELETE FROM {_tableName} WHERE TempStorageItemId IN @tempStorageItemId",
                new { tempStorageItemId },
                cancellationToken
            );

        // v celku nas nezajima, jestli je soubor smazany nebo ne
        try
        {
            foreach (var id in tempStorageItemId)
            {
                await _storageClient.DeleteFile(id.ToString(), cancellationToken: cancellationToken);
            }
        }
        catch { }
    }

    /// <summary>
    /// Kontrola nazvu/koncovky souboru
    /// </summary>
    private void validateFile(TempStorageItem fileInstance)
    {
        // overit validni extension
        if (_configuration.UseAllowedFileExtensions)
        {
            string fileExtension = Path.GetExtension(fileInstance.FileName);
            if (((_configuration.AllowedFileExtensions?.Length ?? 0) == 0 && !_defaultAllowedFileExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                || ((_configuration.AllowedFileExtensions?.Length ?? 0) > 0 && !_configuration.AllowedFileExtensions!.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)))
            {
                throw new TempStorageException(90032, $"File extension {fileExtension} not allowed");
            }
        }

        // overit delku filename
        if (fileInstance.FileName.Length > _configuration.MaxFileNameSize)
        {
            throw new TempStorageException(90038, $"FileName length is greater than {_configuration.MaxFileNameSize}");
        }
    }

    /// <summary>
    /// Ulozeni metadat do databaze
    /// </summary>
    private async Task saveFileToDatabase(TempStorageItem fileInstance, CancellationToken cancellationToken)
    {
        await _connectionProvider.ExecuteDapperAsync(_insertSql, fileInstance, cancellationToken);
    }

    private readonly string[] _defaultAllowedFileExtensions = 
    [
        ".pdf",
        ".png",
        ".txt",
        ".xls",
        ".xlsx",
        ".doc",
        ".docx",
        ".rtf",
        ".jpg",
        ".jpeg",
        ".jfif",
        ".tif",
        ".tiff",
        ".gif"
    ];

    private const string _tableName = "dbo.TempStorageItem";
    private const string _insertSql = $"INSERT INTO {_tableName} (TempStorageItemId, FileName, MimeType, ObjectId, ObjectType, SessionId, TraceId) VALUES (@TempStorageItemId, @FileName, @MimeType, @ObjectId, @ObjectType, @SessionId, @TraceId)";

    private readonly Configuration.TempStorageConfiguration _configuration;
    private readonly IConnectionProvider<ITempStorageConnection> _connectionProvider;
    private readonly IStorageClient<ITempStorage> _storageClient;

    public TempStorage(IConnectionProvider<ITempStorageConnection> connectionProvider, IOptions<Configuration.StorageConfiguration> configuration)
    {
        _connectionProvider = connectionProvider;
        _configuration = configuration.Value.TempStorage!;

        _storageClient = _configuration.StorageClient.StorageType switch
        {
            Configuration.StorageClientTypes.FileSystem => new StorageClients.FileSystemStorageClient<ITempStorage>(_configuration.StorageClient),
            Configuration.StorageClientTypes.AzureBlob => new StorageClients.AzureBlobStorageClient<ITempStorage>(_configuration.StorageClient!),
            Configuration.StorageClientTypes.AmazonS3 => new StorageClients.AmazonS3StorageClient<ITempStorage>(_configuration.StorageClient!),
            _ => throw new CisConfigurationException(0, $"CisStorageServices: configuration type not found for TempStorage client")
        };
    }
}
