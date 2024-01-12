using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharedComponents.Storage.Database;
using System.Diagnostics;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SharedComponents.Storage;

internal class TempStorage
    : ITempStorage
{
    public async Task<TempStorageItem> Save(
        IFormFile file,
        CancellationToken cancellationToken = default)
        => await Save(file, null, null, null, cancellationToken);

    public async Task<TempStorageItem> Save(
        IFormFile file,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        // overit validni extension
        string fileExtension = Path.GetExtension(file.FileName);
        if ((_configuration.AllowedFileExtensions?.Length ?? -1) != 0
            || (_configuration.AllowedFileExtensions ?? _defaultAllowedFileExtensions).Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
        {
            throw new TempStorageException(90032, $"File extension {fileExtension} not allowed");
        }

        // overit delku filename
        if (file.FileName.Length > _configuration.MaxFileNameSize)
        {
            throw new TempStorageException(90038, $"FileName length is greater than {_configuration.MaxFileNameSize}");
        }

        var fileInstance = new TempStorageItem
        {
            TempFileId = Guid.NewGuid(),
            FileName = file.FileName,
            MimeType = file.ContentType,
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType,
            TraceId = Activity.Current?.Id
        };

        // zapsat na disk
        using (var stream = new FileStream(getPath(fileInstance.TempFileId), FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        // ulozit do DB
        string insertSql = $"INSERT INTO {_tableName} (FileName, MimeType, ObjectId, ObjectType, SessionId, TraceId) VALUES (@FileName, @MimeType, @ObjectId, @ObjectType, @SessionId, @TraceId)";
        await _connectionProvider.ExecuteDapperAsync(insertSql, fileInstance, cancellationToken);
        
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

    public async Task<TempStorageItem> GetMetadata(Guid tempFileId, CancellationToken cancellationToken = default)
    {
        return await _connectionProvider
            .ExecuteDapperFirstOrDefaultAsync<TempStorageItem>(
            $"SELECT * FROM {_tableName} WHERE TempStorageItemId=@tempFileId",
            new { tempFileId },
            cancellationToken);
    }

    public async Task<byte[]> GetContent(Guid tempFileId, CancellationToken cancellationToken = default)
    {
        string path = getPath(tempFileId);
        if (!File.Exists(path))
        {
            throw new CisNotFoundException(0, $"TempFileManager: temp file '{path}' not found");
        }

        return await File.ReadAllBytesAsync(path, cancellationToken);
    }

    public async Task Delete(Guid tempFileId, CancellationToken cancellationToken = default)
    {
        await _connectionProvider
            .ExecuteDapperAsync(
                $"DELETE FROM {_tableName} WHERE TempStorageItemId=@tempFileId", 
                new { tempFileId }, 
                cancellationToken
            );
        
        // v celku nas nezajima, jestli je soubor smazany nebo ne
        try
        {
            File.Delete(getPath(tempFileId));
        }
        catch { }
    }

    public async Task Delete(IEnumerable<Guid> tempFileId, CancellationToken cancellationToken = default)
    {
        await _connectionProvider
            .ExecuteDapperAsync(
                $"DELETE FROM {_tableName} WHERE TempStorageItemId IN @tempFileId",
                new { tempFileId },
                cancellationToken
            );

        // v celku nas nezajima, jestli je soubor smazany nebo ne
        try
        {
            foreach (var id in tempFileId)
            {
                File.Delete(getPath(id));
            }
        }
        catch { }
    }

    private string getPath(Guid fileTempId)
        => Path.Combine(_configuration.StoragePath, fileTempId.ToString());

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

    private readonly Configuration.TempStorageConfiguration _configuration;
    private readonly IConnectionProvider<ITempStorageConnection> _connectionProvider;

    public TempStorage(IConnectionProvider<ITempStorageConnection> connectionProvider, IOptions<Configuration.StorageConfiguration> configuration)
    {
        _connectionProvider = connectionProvider;
        _configuration = configuration.Value.TempStorage ?? throw new CisConfigurationNotFound("CisStorage:TempStorage");
    }
}
