using CIS.Core.Security;
using CIS.Core;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Services.DocumentHelper;

namespace NOBY.Services.UploadDocumentToArchive;

[TransientService, AsImplementedInterfacesService]
internal sealed class UploadDocumentToArchiveService
    : IUploadDocumentToArchiveService
{
    private const string _defaultContractNumber = "HF00111111125";

    public async Task<List<string>> Upload(long caseId, string? contractNumber, List<DocumentMetadata> attachments, CancellationToken cancellationToken)
    {
        // instance uzivatele
        var user = await _userServiceClient.GetUser(_currentUserAccessor.User!.Id, cancellationToken);

        var documentIds = new List<string>();

        foreach (var attachment in attachments)
        {
            var documentId = await _documentArchiveService.GenerateDocumentId(new(), cancellationToken);
            var file = await _tempFileManager.GetContent(attachment.TempFileId, cancellationToken);
            var fileMetadata = await _tempFileManager.GetMetadata(attachment.TempFileId, cancellationToken);

            await _documentArchiveService.UploadDocument(new()
            {
                BinaryData = ByteString.CopyFrom(file),
                Metadata = new()
                {
                    CaseId = caseId,
                    DocumentId = documentId,
                    ContractNumber = string.IsNullOrWhiteSpace(contractNumber) ? _defaultContractNumber : contractNumber,
                    CreatedOn = _dateTime.Now.Date,
                    AuthorUserLogin = _documentHelper.GetAuthorUserLoginForDocumentUpload(user),
                    Description = attachment.Description ?? string.Empty,
                    EaCodeMainId = attachment.EaCodeMainId,
                    Filename = fileMetadata.FileName,
                    FormId = attachment.FormId ?? string.Empty
                },
                NotifyStarBuild = false
            }, cancellationToken);

            documentIds.Add(documentId);
        }

        return documentIds;
    }

    private readonly SharedComponents.Storage.ITempStorage _tempFileManager;
    private readonly IDateTime _dateTime;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDocumentHelperService _documentHelper;

    public UploadDocumentToArchiveService(
        SharedComponents.Storage.ITempStorage tempFileManager,
        IDateTime dateTime,
        IDocumentArchiveServiceClient documentArchiveService,
        IUserServiceClient userServiceClient,
        ICurrentUserAccessor currentUserAccessor,
        IDocumentHelperService documentHelper)
    {
        _tempFileManager = tempFileManager;
        _dateTime = dateTime;
        _documentArchiveService = documentArchiveService;
        _userServiceClient = userServiceClient;
        _currentUserAccessor = currentUserAccessor;
        _documentHelper = documentHelper;
    }
}
