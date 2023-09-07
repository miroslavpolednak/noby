using CIS.Core;
using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using CIS.Foms.Enums;
using _DocOnSa = NOBY.Api.Endpoints.DocumentOnSA.Search;
using NOBY.Services.DocumentHelper;
using static DomainServices.CodebookService.Contracts.v1.CodebookService;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveHandler
    : IRequestHandler<SaveDocumentsToArchiveRequest>
{
    private const string defaultContractNumber = "HF00111111125";

    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTime _dateTime;
    private readonly Services.TempFileManager.ITempFileManagerService _tempFileManager;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IMediator _mediator;
    private readonly ICaseServiceClient _caseService;
    private readonly IUserServiceClient _userService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IDocumentHelperService _documentHelper;

    public SaveDocumentToArchiveHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor,
        IDateTime dateTime,
        Services.TempFileManager.ITempFileManagerService tempFileManager,
        ISalesArrangementServiceClient salesArrangementService,
        IDocumentOnSAServiceClient documentOnSAService,
        IMediator mediator,
        ICaseServiceClient caseService,
        IUserServiceClient userService,
        ICodebookServiceClient codebookService,
        IDocumentHelperService documentHelper
        )
    {
        _documentArchiveService = client;
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
        _tempFileManager = tempFileManager;
        _salesArrangementService = salesArrangementService;
        _documentOnSAService = documentOnSAService;
        _mediator = mediator;
        _caseService = caseService;
        _userService = userService;
        _codebookService = codebookService;
        _documentHelper = documentHelper;
    }

    public async Task Handle(SaveDocumentsToArchiveRequest request, CancellationToken cancellationToken)
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

            var file = await _tempFileManager.GetContent(docInfo.DocumentInformation.Guid!.Value, cancellationToken);
            var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);

            filesToUpload.Add(new()
            {
                uploadRequest = await MapRequest(file, documentId, request.CaseId, docInfo, authorUserLogin, cancellationToken),
                documentOnSAId = documentOnSAId
            });
        }

        await Task.WhenAll(filesToUpload.Select(uploadItem => UploadDocument(uploadItem, cancellationToken)));
        await _tempFileManager.Delete(filePaths, cancellationToken);
    }

    private async Task CheckIfDocumentCanByUploadedFromNoby(DocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var eaCodeMainItems = await _codebookService.EaCodesMain(cancellationToken);
        var eACodeMain = eaCodeMainItems.Find(e => e.Id == docInfo.DocumentInformation.EaCodeMainId)
            ?? throw new NobyValidationException($"Unsupported EACodeMainId {docInfo.DocumentInformation.EaCodeMainId}");

        if (eACodeMain?.IsInsertingAllowedNoby == false)
            throw new NobyValidationException($"Document {docInfo.DocumentInformation.FileName} with EACodeMainId {docInfo.DocumentInformation.EaCodeMainId} cannot be uploaded from Noby");
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

    private async Task CheckIfFormIdRequired(DocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);

        var eACodeMain = eACodeMains.FirstOrDefault(r => r.Id == docInfo.DocumentInformation.EaCodeMainId)
            ?? throw new NobyValidationException($"Specified EaCodeMainId: {docInfo.DocumentInformation.EaCodeMainId} isn't valid");

        if (eACodeMain.IsFormIdRequested && string.IsNullOrWhiteSpace(docInfo.FormId))
            throw new NobyValidationException($"For specified EaCodeMainId:{docInfo.DocumentInformation.EaCodeMainId}, FormId is required");
    }

    /// <summary>
    /// If formId exist on DocumentOnSa, return EArchiveId from documentOnSa
    /// </summary>
    /// <returns>DocumentOnSa.DocumentOnSAId</returns>
    /// <exception cref="CisValidationException">Unable to upload file for selected FormId</exception>
    private async Task<int?> ValidateFormId(long caseId, DocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var salesArrangementIdWithFormIdFromDocSa = await CheckIfExistFormIdOnDocumentOnSa(docInfo, caseId, cancellationToken);

        if (salesArrangementIdWithFormIdFromDocSa is null)
        {
            throw new NobyValidationException($"Unable to upload file for selected FormId {docInfo.FormId}");
        }

        var documentsOnSa = await _documentOnSAService.GetDocumentsToSignList(salesArrangementIdWithFormIdFromDocSa.Value, cancellationToken);
        var documentOnSa = documentsOnSa.DocumentsOnSAToSign.Single(d => d.FormId == docInfo.FormId);
        return documentOnSa.DocumentOnSAId;
    }

    /// <summary>
    /// This method try to find formId on DocumentOnsa for all salesarragment under the Case
    /// </summary>
    /// <returns>SalesArrangementId from DocumentOnSa for FormId</returns>
    private async Task<int?> CheckIfExistFormIdOnDocumentOnSa(DocumentsInformation docInfo, long caseId, CancellationToken cancellationToken)
    {
        var salesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        int? salesArrangementIdWithFormIdFromDocSa = null;

        foreach (var salesArrangement in salesArrangements.SalesArrangements)
        {
            var response = await _mediator.Send(new _DocOnSa.SearchDocumentsOnSaRequest
            {
                SalesArrangementId = salesArrangement.SalesArrangementId,
                EACodeMainId = docInfo.DocumentInformation.EaCodeMainId
            }, cancellationToken);

            var findResult = response.FormIds.FirstOrDefault(f => f.FormId == docInfo.FormId);

            if (findResult is not null)
            {
                salesArrangementIdWithFormIdFromDocSa = salesArrangement.SalesArrangementId;
                break;
            }
        }

        return salesArrangementIdWithFormIdFromDocSa;
    }

    private async Task<UploadDocumentRequest> MapRequest(
        byte[] file,
        string documentId,
        long caseId,
        DocumentsInformation documentInformation,
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
                Filename = documentInformation.DocumentInformation.FileName,
                AuthorUserLogin = authorUserLogin,
                CreatedOn = _dateTime.Now.Date,
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
            return defaultContractNumber;

        return caseDetail.Data.ContractNumber;
    }
}
