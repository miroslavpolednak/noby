using CIS.Core;
using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveHandler : IRequestHandler<SaveDocumentsToArchiveRequest>
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

    public async Task<Unit> Handle(SaveDocumentsToArchiveRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var filePaths = new List<string>();
        
        foreach (var docInfo in request.DocumentsInformation)
        {
            var filePath = Path.Combine(_configuration.FileTempFolderLocation, docInfo.Guid!.Value.ToString());

            if (!File.Exists(filePath))
            {
                throw new CisNotFoundException(250, $"Document not found on temp storage {docInfo.Guid}");
            }

            filePaths.Add(filePath);

            var documentId = await _client.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);

            var file = await File.ReadAllBytesAsync(filePath, cancellationToken);

            await _client.UploadDocument(MapRequest(file, documentId, request.CaseId, docInfo), cancellationToken);
        }

        filePaths.ForEach(File.Delete);

        return Unit.Value;
    }

    private UploadDocumentRequest MapRequest(byte[] file, string documentId, long caseId, DocumentsInformation documentInformation)
    {
        return new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(file),
            Kdv = 0,
            Metadata = new()
            {
                CaseId = caseId,
                DocumentId = documentId,
                EaCodeMainId = documentInformation.EaCodeMainId,
                Filename = documentInformation.FileName,
                AuthorUserLogin = _currentUserAccessor.User is not null ? _currentUserAccessor.User.Id.ToString() : "Unknow NOBY user",
                CreatedOn = _dateTime.Now.Date,
                Description = documentInformation.Description
            }
        };
    }
}
