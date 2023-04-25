using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Api.Endpoints.Shared;
using __Contract = DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler : IRequestHandler<GetDocumentListRequest, GetDocumentListResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetDocumentListHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
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

        var documentListResponse = MapGetDocumentListToResponse(getDocumentListResult);
        var documentInQueueRespose = MapgetDocumentsInQueueToRespose(getDocumentsInQueueResult);

        var mergedResponse = MergeDocuments(documentListResponse.DocumentsMetadata, documentInQueueRespose.DocumentsMetadata);

        return null;
    }

    private GetDocumentListResponse MergeDocuments(IReadOnlyCollection<DocumentsMetadata> documentList, IReadOnlyCollection<DocumentsMetadata> documentInQueue)
    {
        var response = new GetDocumentListResponse();
        response.DocumentsMetadata = documentList.Concat(documentInQueue.Where(d => !documentList.Select(l => l.DocumentId)
                                                                        .Contains(d.DocumentId)))
                                                                        .ToList();
        return response;
    }

    private static GetDocumentListResponse MapgetDocumentsInQueueToRespose(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult)
    {
        return new GetDocumentListResponse
        {
            DocumentsMetadata = getDocumentsInQueueResult.QueuedDocuments.Select(s => new DocumentsMetadata
            {
                DocumentId = s.EArchivId,
                EaCodeMainId = s.EaCodeMainId,
                FileName = s.Filename,
                UploadStatus = GetUploadStatus(s.StatusInQueue)
            })
           .ToList()
        };
    }

    private static GetDocumentListResponse MapGetDocumentListToResponse(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return new GetDocumentListResponse
        {
            DocumentsMetadata = getDocumentListResult.Metadata.Select(s => new DocumentsMetadata
            {
                DocumentId = s.DocumentId,
                EaCodeMainId = s.EaCodeMainId,
                FileName = s.Filename,
                Description = s.Description,
                CreatedOn = s.CreatedOn,
                UploadStatus = GetUploadStatus(400) // 400 mean saved in EArchive
            })
            .ToList()
        };
    }

    private static UploadStatus GetUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => UploadStatus.InProgress,
        300 => UploadStatus.Error,
        400 => UploadStatus.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}
