using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts.v1;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Infrastructure.ErrorHandling;
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
                    CreatedOn = _dateTime.GetLocalNow().Date,
                    AuthorUserLogin = _documentHelper.GetAuthorUserLoginForDocumentUpload(user),
                    Description = attachment.Description ?? string.Empty,
                    EaCodeMainId = attachment.EaCodeMainId,
                    Filename = fileMetadata.FileName,
                    FormId = attachment.FormId ?? string.Empty
                },
                NotifyStarBuild = false
            }, cancellationToken);

            // Only TaskTypeId == 6 has not null FormId
            if (!string.IsNullOrWhiteSpace(attachment.FormId))
            {
                var documentOnSaId = await GetDocumentOnSaId(caseId, attachment.FormId, cancellationToken);
                await _documentOnSAService.LinkEArchivIdToDocumentOnSA(new() { DocumentOnSAId = documentOnSaId, EArchivId = documentId }, cancellationToken);
            }
            
            documentIds.Add(documentId);
        }

        return documentIds;
    }

    private async Task<int> GetDocumentOnSaId(long caseId, string formId, CancellationToken cancellationToken)
    {
        var saResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
        foreach (var sa in saResponse.SalesArrangements)
        {
            var docOnSa = (await _documentOnSAService.GetDocumentsOnSAList(sa.SalesArrangementId, cancellationToken))
                .DocumentsOnSA.FirstOrDefault(d => d.IsSigned && d.FormId == formId);

            if (docOnSa is not null)
            {
                return docOnSa.DocumentOnSAId!.Value;
            }
        }

        throw new NobyValidationException(90063, $"FormId:{formId}");
    }

    private readonly SharedComponents.Storage.ITempStorage _tempFileManager;
    private readonly TimeProvider _dateTime;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDocumentHelperService _documentHelper;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public UploadDocumentToArchiveService(
        SharedComponents.Storage.ITempStorage tempFileManager,
        TimeProvider dateTime,
        IDocumentArchiveServiceClient documentArchiveService,
        IUserServiceClient userServiceClient,
        ICurrentUserAccessor currentUserAccessor,
        IDocumentHelperService documentHelper,
        IDocumentOnSAServiceClient documentOnSAService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _tempFileManager = tempFileManager;
        _dateTime = dateTime;
        _documentArchiveService = documentArchiveService;
        _userServiceClient = userServiceClient;
        _currentUserAccessor = currentUserAccessor;
        _documentHelper = documentHelper;
        _documentOnSAService = documentOnSAService;
        _salesArrangementService = salesArrangementService;
    }
}
