using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument;

public sealed class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest>
{
    private readonly DocumentArchiveDbContext _context;

    public UploadDocumentHandler(DocumentArchiveDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        if (await _context.DocumentInterface.AnyAsync(e => e.DocumentId == request.Metadata.DocumentId, cancellationToken))
        {
            throw new CisAlreadyExistsException(14015, "File with documentid already exist in database");
        }

        await _context.DocumentInterface.AddAsync(MapToEntity(request), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static DocumentInterface MapToEntity(UploadDocumentRequest request)
    {
        var entity = new DocumentInterface();
        entity.DocumentId = request.Metadata.DocumentId;
        entity.DocumentData = request.BinaryData.ToByteArray();
        entity.FileName = request.Metadata.Filename;
        entity.FileNameSuffix = Path.GetExtension(request.Metadata.Filename).Substring(1);
        entity.Description = request.Metadata.Description;
        entity.CaseId = request.Metadata.CaseId!.Value;
        entity.CreatedOn = request.Metadata.CreatedOn;
        entity.AuthorUserLogin = request.Metadata.AuthorUserLogin;
        entity.ContractNumber = request.Metadata.ContractNumber;
        entity.FormId = request.Metadata.FormId;
        entity.EaCodeMainId = request.Metadata.EaCodeMainId!.Value;
        // If field is empty, database field default is gonna be set
        if (!string.IsNullOrWhiteSpace(request.Metadata.DocumentDirection))
        {
            entity.DocumentDirection = request.Metadata.DocumentDirection;
        }
        //If field is empty, database field default is gonna be set
        if (!string.IsNullOrWhiteSpace(request.Metadata.FolderDocument))
        {
            entity.FolderDocument = request.Metadata.FolderDocument;
        }
        entity.FolderDocumentId = request.Metadata.FolderDocumentId;
        entity.Kdv = MapToShortWithTrueDefault(request.NotifyStarBuild);
        entity.SendDocumentOnly = MapToShortWithTrueDefault(request.SendDocumentOnly); 
        return entity;
    }

    private static short MapToShortWithTrueDefault(bool? value) => value switch
    {
        true => 1,
        false => 0,
        null => 1
    };
}
