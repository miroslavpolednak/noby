using CIS.Core;
using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf;
using NOBY.Api.Endpoints.Shared;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveHandler
    : IRequestHandler<SaveDocumentsToArchiveRequest>
{
    private readonly AppConfiguration _configuration;
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTime _dateTime;
    private readonly ITempFileManager _tempFileManager;

    public SaveDocumentToArchiveHandler(
        AppConfiguration configuration,
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor,
        IDateTime dateTime,
        ITempFileManager tempFileManager)
    {
        _configuration = configuration;
        _client = client;
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
        _tempFileManager = tempFileManager;
    }

    public async Task Handle(SaveDocumentsToArchiveRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var filePaths = new List<string>();

        foreach (var docInfo in request.DocumentsInformation)
        {
            var filePath = _tempFileManager.ComposeFilePath(docInfo.Guid!.Value.ToString());

            _tempFileManager.CheckIfDocumentExist(filePath);

            filePaths.Add(filePath);

            var documentId = await _client.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);

            var file = await _tempFileManager.GetDocument(filePath, cancellationToken);

            await _client.UploadDocument(MapRequest(file, documentId, request.CaseId, docInfo), cancellationToken);
        }

        _tempFileManager.BatchDelete(filePaths);
    }

    private UploadDocumentRequest MapRequest(byte[] file, string documentId, long caseId, DocumentsInformation documentInformation)
    {
        return new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(file),
            Metadata = new()
            {
                CaseId = caseId,
                DocumentId = documentId,
                EaCodeMainId = documentInformation.EaCodeMainId,
                Filename = documentInformation.FileName,
                AuthorUserLogin = _currentUserAccessor.User is not null ? _currentUserAccessor.User.Id.ToString() : "Unknow NOBY user",
                CreatedOn = _dateTime.Now.Date,
                Description = documentInformation.Description ?? string.Empty,
                FormId = documentInformation.FormId ?? string.Empty,
            }
        };
    }
}
