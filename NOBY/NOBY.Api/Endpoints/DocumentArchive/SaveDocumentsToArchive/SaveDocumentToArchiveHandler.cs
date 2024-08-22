using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.v1;
using Google.Protobuf;
using NOBY.Services.DocumentHelper;
using NOBY.Services.EaCodeMain;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveHandler(
    IDocumentArchiveServiceClient _documentArchiveService,
    ICurrentUserAccessor _currentUserAccessor,
    TimeProvider _dateTime,
    SharedComponents.Storage.ITempStorage _tempFileManager,
    ISalesArrangementServiceClient _salesArrangementService,
    IDocumentOnSAServiceClient _documentOnSAService,
    ICaseServiceClient _caseService,
    IUserServiceClient _userService,
    ICodebookServiceClient _codebookService,
    IDocumentHelperServiceOld _documentHelper,
    IEaCodeMainHelper _eaCodeMainHelper)
        : IRequestHandler<DocumentArchiveSaveDocumentsToArchiveRequest>
{
    private const string _defaultContractNumber = "HF00111111125";

    public async Task Handle(DocumentArchiveSaveDocumentsToArchiveRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var filePaths = new List<Guid>();
        var filesToUpload = new List<(UploadDocumentRequest uploadRequest, int? documentOnSAId)>();
        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        var authorUserLogin = _documentHelper.GetAuthorUserLoginForDocumentUpload(user);

        foreach (var docInfo in request.DocumentsInformation)
        {
            await CheckIfDocumentCanByUploadedFromNoby(docInfo, cancellationToken);

            await CheckDrawingPermissionIfArrangementIsDrawing(docInfo.DocumentInformation.EaCodeMainId, cancellationToken);

            int? documentOnSAId = null;
            await CheckIfFormIdRequired(docInfo, cancellationToken);

            if (!string.IsNullOrWhiteSpace(docInfo.FormId))
                documentOnSAId = await ValidateFormId(request.CaseId, docInfo, cancellationToken);

            filePaths.Add(docInfo.DocumentInformation.Guid!.Value);

            var fileMetadata = await _tempFileManager.GetMetadata(docInfo.DocumentInformation.Guid!.Value, cancellationToken);
            var file = await _tempFileManager.GetContent(docInfo.DocumentInformation.Guid!.Value, cancellationToken);
            
            var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);

            filesToUpload.Add(new()
            {
                uploadRequest = await MapRequest(file, fileMetadata.FileName, documentId, request.CaseId, docInfo, authorUserLogin, cancellationToken),
                documentOnSAId = documentOnSAId
            });
        }

        await Task.WhenAll(filesToUpload.Select(uploadItem => UploadDocument(uploadItem, cancellationToken)));
        await _tempFileManager.Delete(filePaths, cancellationToken);
    }

    private async Task CheckIfDocumentCanByUploadedFromNoby(DocumentArchiveDocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var eaCodeMainItems = await _codebookService.EaCodesMain(cancellationToken);
        var eACodeMain = eaCodeMainItems.Find(e => e.Id == docInfo.DocumentInformation.EaCodeMainId)
            ?? throw new NobyValidationException($"Unsupported EACodeMainId {docInfo.DocumentInformation.EaCodeMainId}");

        if (eACodeMain?.IsInsertingAllowedNoby == false)
            throw new NobyValidationException($"Document {docInfo.DocumentInformation.Guid} with EACodeMainId {docInfo.DocumentInformation.EaCodeMainId} cannot be uploaded from Noby");
    }

    private async Task UploadDocument((UploadDocumentRequest uploadRequest, int? documentOnSAId) uploadItem, CancellationToken cancellationToken)
    {
        await _documentArchiveService.UploadDocument(uploadItem.uploadRequest, cancellationToken);
        if (uploadItem.documentOnSAId != null)
            await _documentOnSAService.LinkEArchivIdToDocumentOnSA(
                new()
                {
                    DocumentOnSAId = uploadItem.documentOnSAId.Value,
                    EArchivId = uploadItem.uploadRequest.Metadata.DocumentId
                },
                        cancellationToken);
    }

    private async Task CheckDrawingPermissionIfArrangementIsDrawing(int? eaCodeMainId, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);

        if (!documentTypes.Any(d => d.SalesArrangementTypeId == (int)SalesArrangementTypes.Drawing && d.EACodeMainId == eaCodeMainId))
            return;

        if (_currentUserAccessor.HasPermission(UserPermissions.SIGNING_DOCUMENT_UploadDrawingDocument))
            return;

        throw new CisAuthorizationException("CheckDrawingPermissionIfArrangementIsDrawing failed");
    }

    private async Task CheckIfFormIdRequired(DocumentArchiveDocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);

        var eACodeMain = eACodeMains.FirstOrDefault(r => r.Id == docInfo.DocumentInformation.EaCodeMainId)
            ?? throw new NobyValidationException($"Specified EaCodeMainId: {docInfo.DocumentInformation.EaCodeMainId} isn't valid");

        if (eACodeMain.IsFormIdRequested && string.IsNullOrWhiteSpace(docInfo.FormId))
            throw new NobyValidationException($"For specified EaCodeMainId:{docInfo.DocumentInformation.EaCodeMainId}, FormId is required");
    }

    /// <summary>
    /// If formId exist on DocumentOnSa, return DocumentOnSAId (for EArchivIdsLinked), if not throw exp
    /// </summary>
    /// <returns>DocumentOnSa.DocumentOnSAId</returns>
    /// <exception cref="CisValidationException">Unable to upload file for selected FormId</exception>
    private async Task<int?> ValidateFormId(long caseId, DocumentArchiveDocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var eaCodeMainId = docInfo.DocumentInformation.EaCodeMainId!.Value;

        await _eaCodeMainHelper.ValidateEaCodeMain(eaCodeMainId, cancellationToken);

        var saResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        foreach (var salesArragment in saResponse.SalesArrangements)
        {
            var documentsOnSaResponse = await _documentOnSAService.GetDocumentsOnSAList(salesArragment.SalesArrangementId, cancellationToken);
            var documentTypesForEaCodeMain = await _eaCodeMainHelper.GetDocumentTypeIdsAccordingEaCodeMain(eaCodeMainId, cancellationToken);

            var documentsOnSaFiltered = documentsOnSaResponse.DocumentsOnSA
               .Where(f => documentTypesForEaCodeMain.Contains(f.DocumentTypeId!.Value)
                           && !string.IsNullOrWhiteSpace(f.FormId)
                           && !f.IsFinal
                           && f.IsSigned);

            var documentOnSa = documentsOnSaFiltered.FirstOrDefault(r => r.FormId == docInfo.FormId);

            if (documentOnSa is not null)
            {
                return documentOnSa.DocumentOnSAId;
            }
        }

        throw new NobyValidationException($"Unable to upload file, there isn't valid signed document for specified FormId {docInfo.FormId}");
    }

    private async Task<UploadDocumentRequest> MapRequest(
        byte[] file,
        string fileName,
        string documentId,
        long caseId,
        DocumentArchiveDocumentsInformation documentInformation,
        string authorUserLogin,
        CancellationToken cancellationToken)
    {
        return new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(file),
            Metadata = new()
            {
                CaseId = caseId,
                DocumentId = documentId,
                EaCodeMainId = documentInformation.DocumentInformation.EaCodeMainId,
                Filename = fileName,
                AuthorUserLogin = authorUserLogin,
                CreatedOn = _dateTime.GetLocalNow().Date,
                Description = documentInformation.DocumentInformation.Description ?? string.Empty,
                FormId = documentInformation.FormId ?? string.Empty,
                ContractNumber = await GetContractNumber(caseId, cancellationToken)
            }
        };
    }

    private async Task<string> GetContractNumber(long caseId, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);
        if (string.IsNullOrWhiteSpace(caseDetail?.Data?.ContractNumber))
            return _defaultContractNumber;

        return caseDetail.Data.ContractNumber;
    }
}
