using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public class GetCaseDocumentsFlagHandler : IRequestHandler<GetCaseDocumentsFlagRequest, GetCaseDocumentsFlagResponse>
{
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClient;

    public GetCaseDocumentsFlagHandler(
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        ICodebookServiceClients codebookServiceClient
        )
    {
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _codebookServiceClient = codebookServiceClient;
    }

    public async Task<GetCaseDocumentsFlagResponse> Handle(GetCaseDocumentsFlagRequest request, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _documentArchiveServiceClient.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var eaCodeMain = await _codebookServiceClient.EaCodesMain(cancellationToken);

        // Filter
        var documentsInQueueFiltered = getDocumentsInQueueResult.QueuedDocuments.Select(data =>
        new
        {
            docData = data,
            eACodeMainObj = eaCodeMain.FirstOrDefault(r => r.Id == data.EaCodeMainId)
        })
        .Where(f => f.eACodeMainObj is not null && f.eACodeMainObj.IsVisibleForKb)
        .Select(s => s.docData).ToList();

        return new GetCaseDocumentsFlagResponse
        {
            DocumentsMenuItem = new DocumentsMenuItem
            {
                Flag = GetDocumentsFlag(documentsInQueueFiltered)
            }
        };
    }

    private static FlagDocuments GetDocumentsFlag(List<QueuedDocument> queuedDocuments)
    {
        if (queuedDocuments.Any(s => s.StatusInQueue == 300))
        {
            return FlagDocuments.Error;
        }
        else if (queuedDocuments.Any(s => s.StatusInQueue == 100 || s.StatusInQueue == 110 || s.StatusInQueue == 200))
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

