using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public class GetCaseDocumentsFlagHandler : IRequestHandler<GetCaseDocumentsFlagRequest, GetCaseDocumentsFlagResponse>
{
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly IDocumentHelper _documentHelper;

    public GetCaseDocumentsFlagHandler(
            IDocumentArchiveServiceClient documentArchiveServiceClient,
            IDocumentHelper documentHelper
            )
    {
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _documentHelper = documentHelper;
    }

    public async Task<GetCaseDocumentsFlagResponse> Handle(GetCaseDocumentsFlagRequest request, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _documentArchiveServiceClient.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var getDocumentsInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var documentsInQueueFiltered = await _documentHelper.FilterDocumentsVisibleForKb(getDocumentsInQueueMetadata, cancellationToken);

        return new GetCaseDocumentsFlagResponse
        {
            DocumentsMenuItem = new DocumentsMenuItem
            {
                Flag = GetDocumentsFlag(documentsInQueueFiltered.ToList())
            }
        };
    }

    private static FlagDocuments GetDocumentsFlag(List<DocumentsMetadata> queuedDocuments)
    {
        if (queuedDocuments.Any(s => s.UploadStatus == UploadStatus.Error))
        {
            return FlagDocuments.Error;
        }
        else if (queuedDocuments.Any(s => s.UploadStatus == UploadStatus.InProgress))
        {
            return FlagDocuments.InProcessing;
        }
        else if (!queuedDocuments.Any())
        {
            return FlagDocuments.NoFlag;
        }
        else
        {
            throw new ArgumentException("This state isn't supported");
        }
    }
}

