using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharedComponents.Storage.Database;
using System.Diagnostics;

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
        using (var stream = new FileStream(getPath(fileInstance.TempStorageItemId), FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        // ulozit do DB
        await saveFileToDatabase(fileInstance, cancellationToken);
        
        return fileInstance;
    }

    public async Task<TempStorageItem> Save(
        byte[] fileData,
        string mimeType,
        string fileName,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        var fileInstance = new TempStorageItem
        {
            TempStorageItemId = Guid.NewGuid(),
            FileName = fileName,
            MimeType = mimeType,
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType,
            TraceId = Activity.Current?.Id
        };

        validateFile(fileInstance);

        // zapsat na disk
        await File.WriteAllBytesAsync(getPath(fileInstance.TempStorageItemId), fileData, cancellationToken);

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
        string path = getPath(tempStorageItemId);
        if (!File.Exists(path))
        {
            throw new CisNotFoundException(0, $"TempFileManager: temp file '{path}' not found");
        }

        return await File.ReadAllBytesAsync(path, cancellationToken);
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
            File.Delete(getPath(tempStorageItemId));
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
                File.Delete(getPath(id));
            }
        }
        catch { }
    }

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

    private async Task saveFileToDatabase(TempStorageItem fileInstance, CancellationToken cancellationToken)
    {
        // ulozit do DB
        string insertSql = $"INSERT INTO {_tableName} (TempStorageItemId, FileName, MimeType, ObjectId, ObjectType, SessionId, TraceId) VALUES (@TempStorageItemId, @FileName, @MimeType, @ObjectId, @ObjectType, @SessionId, @TraceId)";
        await _connectionProvider.ExecuteDapperAsync(insertSql, fileInstance, cancellationToken);
    }

    private string getPath(Guid tempStorageItemId)
        => Path.Combine(_configuration.StoragePath, tempStorageItemId.ToString());

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
        _configuration = configuration.Value.TempStorage!;
    }
}
