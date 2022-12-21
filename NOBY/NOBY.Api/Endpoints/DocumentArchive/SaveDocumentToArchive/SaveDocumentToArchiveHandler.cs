using CIS.Core;
using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentToArchive;

public class SaveDocumentToArchiveHandler : IRequestHandler<SaveDocumentToArchiveRequest>
{
    private readonly AppConfiguration _configuration;
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTime _dateTime;

    public SaveDocumentToArchiveHandler(
        AppConfiguration configuration,
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor,
        IDateTime dateTime)
    {
        _configuration = configuration;
        _client = client;
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
    }

    public async Task<Unit> Handle(SaveDocumentToArchiveRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var filePath = Path.Combine(_configuration.FileTempFolderLocation, request.Guid!.Value.ToString());

        if (!File.Exists(filePath))
        {
            throw new CisNotFoundException(250, $"Document not found on temp storage");
        }

        var documentId = await _client.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);
        
        var file = await File.ReadAllBytesAsync(filePath, cancellationToken);

        await _client.UploadDocument(new()
        {
            BinaryData = ByteString.CopyFrom(file),
            Kdv = 0,
            Metadata = new()
            {
                CaseId = request.CaseId,
                DocumentId = documentId,
                EaCodeMainId = request.EaCodeMainId,
                Filename = request.FileName,
                AuthorUserLogin = _currentUserAccessor.User is not null ? _currentUserAccessor.User.Id.ToString() : "Unknow NOBY user",
                CreatedOn = _dateTime.Now.Date
            }

        }, cancellationToken);

        File.Delete(filePath);

        return Unit.Value;
    }
}
