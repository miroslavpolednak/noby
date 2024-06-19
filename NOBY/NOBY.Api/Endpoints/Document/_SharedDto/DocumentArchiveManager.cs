using CIS.Core.Attributes;
using CIS.Core.Configuration;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Services.DocumentHelper;
using System.Globalization;

namespace NOBY.Api.Endpoints.Document.SharedDto;

[TransientService, SelfService]
internal sealed class DocumentArchiveManager
{
    // Document has been successfully transferred to Archive
    public const int QueueStateSuccess = 400;

    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICisEnvironmentConfiguration _environmentConfiguration;
    private readonly IDocumentHelperService _documentHelperService;
    private readonly IUserServiceClient _userService;

    public DocumentArchiveManager(IDocumentArchiveServiceClient documentArchiveService,
                                  ICodebookServiceClient codebookService,
                                  ICisEnvironmentConfiguration environmentConfiguration,
                                  IDocumentHelperService documentHelperService,
                                  IUserServiceClient userService)
    {
        _documentArchiveService = documentArchiveService;
        _codebookService = codebookService;
        _environmentConfiguration = environmentConfiguration;
        _documentHelperService = documentHelperService;
        _userService = userService;
    }

    public Task<string> GenerateDocumentId(CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(typeof(EnvironmentNames), _environmentConfiguration.EnvironmentName!, true, out var enumValue))
            enumValue = EnvironmentNames.Unknown;

        var request = new GenerateDocumentIdRequest { EnvironmentName = (EnvironmentNames)enumValue };

        return _documentArchiveService.GenerateDocumentId(request, cancellationToken);
    }

    public async Task<ReadOnlyMemory<byte>> GetDocument(string documentId, GetDocumentBaseRequest documentRequest, CancellationToken cancellationToken)
    {
        return await CheckIfDocWasTransferredToEArchive(documentId, cancellationToken)
            ? await LoadFromEArchive(documentId, documentRequest, cancellationToken)
            : await LoadFromEArchiveQueue(documentId, cancellationToken);
    }

    private async Task<ReadOnlyMemory<byte>> LoadFromEArchiveQueue(string documentId, CancellationToken cancellationToken)
    {
        var queueRequest = new GetDocumentsInQueueRequest();
        queueRequest.EArchivIds.Add(documentId);
        queueRequest.WithContent = true;

        var documentsInQueue = await _documentArchiveService.GetDocumentsInQueue(queueRequest, cancellationToken);
        return documentsInQueue.QueuedDocuments.Single(r => r.EArchivId == documentId).DocumentData.Memory;
    }

    private async Task<ReadOnlyMemory<byte>> LoadFromEArchive(string documentId, GetDocumentBaseRequest documentRequest, CancellationToken cancellationToken)
    {
        var request = new GetDocumentRequest
        {
            DocumentId = documentId,
            UserLogin = documentRequest.InputParameters.UserId?.ToString(CultureInfo.InvariantCulture) ?? "",
            WithContent = true
        };

        var response = await _documentArchiveService.GetDocument(request, cancellationToken);
        documentRequest.InputParameters.CaseId = response.Metadata.CaseId;
        return response.Content.BinaryData.Memory;
    }

    private async Task<bool> CheckIfDocWasTransferredToEArchive(string documentId, CancellationToken cancellationToken)
    {
        var queueRequest = new GetDocumentsInQueueRequest();
        queueRequest.EArchivIds.Add(documentId);

        var documentsInQueue = await _documentArchiveService.GetDocumentsInQueue(queueRequest, cancellationToken);

        var documentInQueue = documentsInQueue.QueuedDocuments.FirstOrDefault(r => r.EArchivId == documentId)
            ?? throw new NobyValidationException("Unable to find document in EArchive queue");

        return documentInQueue.StatusInQueue == QueueStateSuccess;
    }

    public async Task SaveDocumentToArchive(DocumentArchiveData archiveData, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var user = await _userService.GetUser(archiveData.UserId, cancellationToken);

        var request = new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(archiveData.DocumentData.Span),
            Metadata = new DocumentMetadata
            {
                DocumentId = archiveData.DocumentId,
                CaseId = archiveData.CaseId,
                AuthorUserLogin = _documentHelperService.GetAuthorUserLoginForDocumentUpload(user),
                EaCodeMainId = documentTypes.First(d => d.Id == archiveData.DocumentTypeId).EACodeMainId,
                Filename = archiveData.FileName,
                CreatedOn = DateTime.Now,
                ContractNumber = string.IsNullOrWhiteSpace(archiveData.ContractNumber) ? "HF00111111125" : archiveData.ContractNumber
            }
        };

        await _documentArchiveService.UploadDocument(request, cancellationToken);
    }
}