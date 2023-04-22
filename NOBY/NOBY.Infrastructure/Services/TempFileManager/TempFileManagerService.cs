using CIS.Core;
using CIS.Core.Attributes;
using CIS.Core.Exceptions;
using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Infrastructure.Configuration;
using System.Globalization;

namespace NOBY.Infrastructure.Services.TempFileManager;

[TransientService, AsImplementedInterfacesService]
internal sealed class TempFileManagerService
    : ITempFileManager
{
    public void BatchDelete(List<TempDocumentInformation>? attachments)
    {
        if (attachments is null || !attachments.Any()) return;

        BatchDelete(attachments.Select(t => ComposeFilePath(t.TempGuid)).ToList());
    }

    public void BatchDelete(List<string> filePaths)
    {
        filePaths.ForEach(File.Delete);
    }

    public void CheckIfDocumentExist(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new CisNotFoundException(250, $"Document not found on temp storage");
        }
    }

    public string ComposeFilePath(string fileName)
    {
        return Path.Combine(_configuration.FileTempFolderLocation, fileName);
    }

    public string ComposeFilePath(Guid tempGuid)
    {
        return Path.Combine(_configuration.FileTempFolderLocation, tempGuid.ToString());
    }

    public async Task<byte[]> GetDocument(string filePath, CancellationToken cancellationToken)
    {
        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task<List<string>> UploadToArchive(long caseId, string? contractNumber, List<TempDocumentInformation> attachments, CancellationToken cancellationToken)
    {
        // instance uzivatele
        var user = await _userServiceClient.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        
        var documentIds = new List<string>();

        foreach (var attachment in attachments)
        {
            var documentId = await _documentArchiveService.GenerateDocumentId(new(), cancellationToken);

            var filePath = ComposeFilePath(attachment.TempGuid);
            CheckIfDocumentExist(filePath);

            var file = await GetDocument(filePath, cancellationToken);

            await _documentArchiveService.UploadDocument(new()
            {
                BinaryData = ByteString.CopyFrom(file),
                Metadata = new()
                {
                    CaseId = caseId,
                    DocumentId = documentId,
                    ContractNumber = contractNumber ?? "HF00111111125",
                    CreatedOn = _dateTime.Now.Date,
                    AuthorUserLogin = user.CPM ?? user.Id.ToString(CultureInfo.InvariantCulture),
                    Description = attachment.Description ?? string.Empty,
                    EaCodeMainId = attachment.EaCodeMainId,
                    Filename = attachment.FileName,
                    FormId = attachment.FormId ?? string.Empty
                },
                NotifyStarBuild = false
            }, cancellationToken);

            documentIds.Add(documentId);
        }

        return documentIds;
    }

    private readonly AppConfiguration _configuration;
    private readonly IDateTime _dateTime;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public TempFileManagerService(
        AppConfiguration configuration,
        IDateTime dateTime,
        IDocumentArchiveServiceClient documentArchiveService,
        IUserServiceClient userServiceClient,
        ICurrentUserAccessor currentUserAccessor)
    {
        _configuration = configuration;
        _dateTime = dateTime;
        _documentArchiveService = documentArchiveService;
        _userServiceClient = userServiceClient;
        _currentUserAccessor = currentUserAccessor;
    }
}

