using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.v1;
using Google.Protobuf;
using NOBY.Services.DocumentHelper;

namespace NOBY.Services.UploadDocumentToArchive;

[TransientService, AsImplementedInterfacesService]
internal sealed class UploadDocumentToArchiveService(
    SharedComponents.Storage.ITempStorage _tempFileManager,
    TimeProvider _dateTime,
    IDocumentArchiveServiceClient _documentArchiveService,
    IUserServiceClient _userServiceClient,
    ICurrentUserAccessor _currentUserAccessor,
    IDocumentHelperServiceOld _documentHelper,
    IDocumentOnSAServiceClient _documentOnSAService,
    ISalesArrangementServiceClient _salesArrangementService)
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
                if (documentOnSaId.HasValue)
                {
                    await _documentOnSAService.LinkEArchivIdToDocumentOnSA(new() 
                    { 
                        DocumentOnSAId = documentOnSaId.Value, 
                        EArchivId = documentId 
                    }, cancellationToken);
                }
            }
            
            documentIds.Add(documentId);
        }

        return documentIds;
    }

    private async Task<int?> GetDocumentOnSaId(long caseId, string formId, CancellationToken cancellationToken)
    {
        var saResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
        foreach (var sa in saResponse.SalesArrangements)
        {
            var docOnSa = (await _documentOnSAService.GetDocumentsOnSAList(sa.SalesArrangementId, cancellationToken))
                .DocumentsOnSA
                .FirstOrDefault(d => d.FormId == formId);

            if (docOnSa is not null)
            {
                return docOnSa.DocumentOnSAId!.Value;
            }
        }

        return null;
    }
}
