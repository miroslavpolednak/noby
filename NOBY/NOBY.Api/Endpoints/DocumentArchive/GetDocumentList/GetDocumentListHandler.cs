using CIS.Core.Security;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Api.Endpoints.Shared;
using __Contract = DomainServices.DocumentArchiveService.Contracts;
using __Api = NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler : IRequestHandler<GetDocumentListRequest, GetDocumentListResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICodebookServiceClients _codebookServiceClient;

    public GetDocumentListHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor,
        ICodebookServiceClients codebookServiceClient)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
        _codebookServiceClient = codebookServiceClient;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var getDocumentListResult = await _client.GetDocumentList(new()
        {
            CaseId = request.CaseId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),

        }, cancellationToken);

        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _client.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = MapGetDocumentListMetadata(getDocumentListResult);
        var documentInQueueMetadata = MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);

        var mergedDocumentMetadata = MergeDocuments(documentListMetadata, documentInQueueMetadata);

        var eaCodeMain = await _codebookServiceClient.EaCodesMain(cancellationToken);

        // Filter
        var mergedDocumentMetadataFiltered = mergedDocumentMetadata.Select(data =>
        new
        {
            docData = data,
            eACodeMainObj = eaCodeMain.FirstOrDefault(r => r.Id == data.EaCodeMainId)
        })
        .Where(f => f.eACodeMainObj is not null && f.eACodeMainObj.IsVisibleForKb);

        var finalResponse = new GetDocumentListResponse();
        finalResponse.DocumentsMetadata = mergedDocumentMetadataFiltered.Select(s => s.docData).ToList();
        return finalResponse;
    }

    private static IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<__Api.DocumentsMetadata> documentList, IEnumerable<__Api.DocumentsMetadata> documentInQueue)
    {
        return documentList.Concat(documentInQueue.Where(d => !documentList.Select(l => l.DocumentId)
                                                                        .Contains(d.DocumentId)));
    }

    private static IEnumerable<__Api.DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult)
    {
        return getDocumentsInQueueResult.QueuedDocuments.Select(s => new DocumentsMetadata
        {
            DocumentId = s.EArchivId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            UploadStatus = UploadStatusHelper.GetUploadStatus(s.StatusInQueue)
        });
    }

    private static IEnumerable<__Api.DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return getDocumentListResult.Metadata.Select(s => new DocumentsMetadata
        {
            DocumentId = s.DocumentId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            Description = s.Description,
            CreatedOn = s.CreatedOn,
            UploadStatus = UploadStatusHelper.GetUploadStatus(400) // 400 mean saved in EArchive
        });
    }
}
