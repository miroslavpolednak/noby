using CIS.Core;
using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Api.Endpoints.Shared;
using System.Globalization;
using _DocOnSa = NOBY.Api.Endpoints.DocumentOnSA.Search;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveHandler
    : IRequestHandler<SaveDocumentsToArchiveRequest>
{
    private const string defaultContractNumber = "HF00111111125";

    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTime _dateTime;
    private readonly ITempFileManager _tempFileManager;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;
    private readonly IDocumentOnSAServiceClient _documentOnSAServiceClient;
    private readonly IMediator _mediator;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IUserServiceClient _userServiceClient;

    public SaveDocumentToArchiveHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor,
        IDateTime dateTime,
        ITempFileManager tempFileManager,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        IDocumentOnSAServiceClient documentOnSAServiceClient,
        IMediator mediator,
        ICaseServiceClient caseServiceClient,
        IUserServiceClient userServiceClient)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
        _tempFileManager = tempFileManager;
        _salesArrangementServiceClient = salesArrangementServiceClient;
        _documentOnSAServiceClient = documentOnSAServiceClient;
        _mediator = mediator;
        _caseServiceClient = caseServiceClient;
        _userServiceClient = userServiceClient;
    }

    public async Task Handle(SaveDocumentsToArchiveRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var filePaths = new List<string>();
        var filesToUpload = new List<UploadDocumentRequest>();

        foreach (var docInfo in request.DocumentsInformation)
        {
            var eArchiveIdFromDocumentOnSa = string.Empty;
            if (!string.IsNullOrWhiteSpace(docInfo.FormId))
                eArchiveIdFromDocumentOnSa = await ValidateFormId(request.CaseId, docInfo, cancellationToken);

            var filePath = _tempFileManager.ComposeFilePath(docInfo.Guid!.Value.ToString());
            _tempFileManager.CheckIfDocumentExist(filePath);
            filePaths.Add(filePath);

            var file = await _tempFileManager.GetDocument(filePath, cancellationToken);

            var documentId = string.IsNullOrWhiteSpace(eArchiveIdFromDocumentOnSa) ?
                await _client.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken)
                : eArchiveIdFromDocumentOnSa;

            filesToUpload.Add(await MapRequest(file, documentId, request.CaseId, docInfo, cancellationToken));
        }

        await Task.WhenAll(filesToUpload.Select(file => _client.UploadDocument(file, cancellationToken)));
        _tempFileManager.BatchDelete(filePaths);
    }

    /// <summary>
    /// If formId exist on DocumentOnSa, return EArchiveId from documentOnSa
    /// </summary>
    /// <returns>DocumentOnSa.EArchiveId</returns>
    /// <exception cref="CisValidationException">Unable to upload file for selected FormId</exception>
    private async Task<string> ValidateFormId(long caseId, DocumentsInformation docInfo, CancellationToken cancellationToken)
    {
        var salesArrangementIdWithFormIdFromDocSa = await CheckIfExistFormIdOnDocumentOnSa(docInfo, caseId, cancellationToken);

        if (salesArrangementIdWithFormIdFromDocSa is null)
        {
            throw new CisValidationException(90001, $"Unable to upload file for selected FormId {docInfo.FormId}");
        }

        var documentsOnSa = await _documentOnSAServiceClient.GetDocumentsToSignList(salesArrangementIdWithFormIdFromDocSa.Value, cancellationToken);
        var documentOnSa = documentsOnSa.DocumentsOnSAToSign.Single(d => d.FormId == docInfo.FormId);
        return documentOnSa.EArchivId;
    }

    /// <summary>
    /// This method try to find formId on DocumentOnsa for all salesarragment under the Case
    /// </summary>
    /// <returns>SalesArrangementId from DocumentOnSa for FormId</returns>
    private async Task<int?> CheckIfExistFormIdOnDocumentOnSa(DocumentsInformation docInfo, long caseId, CancellationToken cancellationToken)
    {
        var salesArrangements = await _salesArrangementServiceClient.GetSalesArrangementList(caseId, cancellationToken);

        int? salesArrangementIdWithFormIdFromDocSa = null;

        foreach (var salesArrangement in salesArrangements.SalesArrangements)
        {
            var response = await _mediator.Send(new _DocOnSa.SearchRequest
            {
                SalesArrangementId = salesArrangement.SalesArrangementId,
                EACodeMainId = docInfo.EaCodeMainId
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

    private async Task<UploadDocumentRequest> MapRequest(byte[] file, string documentId, long caseId, DocumentsInformation documentInformation, CancellationToken cancellationToken)
    {
        var user = await _userServiceClient.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        
        return new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(file),
            Metadata = new()
            {
                CaseId = caseId,
                DocumentId = documentId,
                EaCodeMainId = documentInformation.EaCodeMainId,
                Filename = documentInformation.FileName,
                AuthorUserLogin = user.CPM ?? user.Id.ToString(CultureInfo.InvariantCulture),
                CreatedOn = _dateTime.Now.Date,
                Description = documentInformation.Description ?? string.Empty,
                FormId = documentInformation.FormId ?? string.Empty,
                ContractNumber = await GetContractNumber(caseId, cancellationToken)
            }
        };
    }

    private async Task<string> GetContractNumber(long caseId, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseServiceClient.GetCaseDetail(caseId, cancellationToken);
        if (string.IsNullOrWhiteSpace(caseDetail?.Data?.ContractNumber))
            return defaultContractNumber;
        
        return caseDetail.Data.ContractNumber;
    }
}
