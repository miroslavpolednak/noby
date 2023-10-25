﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Services.TempFileManager;

[TransientService, AsImplementedInterfacesService]
internal class TempFileManagerService
    : ITempFileManagerService
{
    public async Task<TempFile> Save(
        IFormFile file,
        CancellationToken cancellationToken = default)
        => await Save(file, null, null, null, cancellationToken);

    public async Task<TempFile> Save(
        IFormFile file,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default)
    {
        // overit validni extension
        if (!_allowedFileExtensions.Contains(Path.GetExtension(file.FileName), StringComparer.OrdinalIgnoreCase))
        {
            throw new NOBY.Infrastructure.ErrorHandling.NobyValidationException(90032);
        }

        // overit delku filename
        if (file.FileName.Length > 64)
        {
            throw new NOBY.Infrastructure.ErrorHandling.NobyValidationException(90038);
        }

        var fileInstance = new TempFile
        {
            TempFileId = Guid.NewGuid(),
            FileName = file.FileName,
            MimeType = file.ContentType,
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType
        };

        // zapsat na disk
        using (var stream = new FileStream(getPath(fileInstance.TempFileId), FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        // ulozit do DB
        var entity = new Database.Entities.TempFile
        {
            TempFileId = fileInstance.TempFileId,
            FileName = file.FileName,
            MimeType = file.ContentType,
            SessionId = sessionId,
            ObjectId = objectId,
            ObjectType = objectType
        };
        _dbContext.TempFiles.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return fileInstance;
    }

    public async Task<List<TempFile>> GetSession(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TempFiles
            .AsNoTracking()
            .Where(t => t.SessionId == sessionId)
            .Select(t => new TempFile
            {
                TempFileId = t.TempFileId,
                FileName = t.FileName,
                SessionId = t.SessionId,
                ObjectId = t.ObjectId,
                ObjectType = t.ObjectType,
                MimeType = t.MimeType
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<TempFile> GetMetadata(Guid tempFileId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TempFiles
            .AsNoTracking()
            .Where(t => t.TempFileId == tempFileId)
            .Select(t => new TempFile
            {
                TempFileId = tempFileId,
                FileName = t.FileName,
                SessionId = t.SessionId,
                ObjectId = t.ObjectId,
                ObjectType = t.ObjectType,
                MimeType = t.MimeType
            })
            .FirstAsync(cancellationToken);
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
        await _dbContext.TempFiles
            .Where(t => t.TempFileId == tempFileId)
            .ExecuteDeleteAsync(cancellationToken);

        // v celku nas nezajima, jestli je soubor smazany nebo ne
        try
        {
            File.Delete(getPath(tempFileId));
        }
        catch { }
    }

    public async Task Delete(IEnumerable<Guid> tempFileId, CancellationToken cancellationToken = default)
    {
        await _dbContext.TempFiles
            .Where(t => tempFileId.Contains(t.TempFileId))
            .ExecuteDeleteAsync(cancellationToken);

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
        => Path.Combine(_appConfiguration.FileTempFolderLocation, fileTempId.ToString());

    private readonly HashSet<string> _allowedFileExtensions = new()
    {
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
    };

    private readonly AppConfiguration _appConfiguration;
    private readonly Database.FeApiDbContext _dbContext;

    public TempFileManagerService(Database.FeApiDbContext dbContext, AppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
        _dbContext = dbContext;
    }
}

