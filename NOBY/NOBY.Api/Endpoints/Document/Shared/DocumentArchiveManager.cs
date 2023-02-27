using CIS.Core.Attributes;
using CIS.Core.Configuration;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf;
using NOBY.Api.Endpoints.Document.Shared.DocumentIdManager;

namespace NOBY.Api.Endpoints.Document.Shared;

[TransientService, SelfService]
internal class DocumentArchiveManager<TDocumentIdManager, TEntityId> where TDocumentIdManager : IDocumentIdManager<TEntityId>
{
    private readonly TDocumentIdManager _documentIdManager;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICisEnvironmentConfiguration _environmentConfiguration;

    public DocumentArchiveManager(TDocumentIdManager documentIdManager,
                                  IDocumentArchiveServiceClient documentArchiveService,
                                  ICisEnvironmentConfiguration environmentConfiguration)
    {
        _documentIdManager = documentIdManager;
        _documentArchiveService = documentArchiveService;
        _environmentConfiguration = environmentConfiguration;
    }

    public Task<string> GetDocumentId(TEntityId entityId, CancellationToken cancellationToken) =>
        _documentIdManager.LoadDocumentId(entityId, cancellationToken);

    public Task<string> GenerateDocumentId(CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(typeof(EnvironmentNames), _environmentConfiguration.EnvironmentName!, true, out var enumValue))
            enumValue = EnvironmentNames.Unknown;

        var request = new GenerateDocumentIdRequest { EnvironmentName = (EnvironmentNames)enumValue };

        return _documentArchiveService.GenerateDocumentId(request, cancellationToken);
    }

    public async Task<ReadOnlyMemory<byte>> GetDocument(string documentId, GetDocumentBaseRequest documentRequest, CancellationToken cancellationToken)
    {
        var request = new GetDocumentRequest
        {
            DocumentId = documentId,
            UserLogin = documentRequest.InputParameters.UserId.ToString(),
            WithContent = true
        };

        var response = await _documentArchiveService.GetDocument(request, cancellationToken);

        documentRequest.InputParameters.CaseId = response.Metadata.CaseId;

        return response.Content.BinaryData.Memory;
    }

    public async Task SaveDocumentToArchive(TEntityId entityId, DocumentArchiveData archiveData, CancellationToken cancellationToken)
    {
        var request = new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(archiveData.DocumentData.Span),
            Metadata = new DocumentMetadata
            {
                DocumentId = archiveData.DocumentId,
                CaseId = archiveData.CaseId,
                AuthorUserLogin = archiveData.UserId.ToString(),
                EaCodeMainId = 605469,
                Filename = archiveData.FileName,
                CreatedOn = DateTime.Now
            }
        };

        await _documentArchiveService.UploadDocument(request, cancellationToken);

        await _documentIdManager.UpdateDocumentId(entityId, archiveData.DocumentId, cancellationToken);
    }
}