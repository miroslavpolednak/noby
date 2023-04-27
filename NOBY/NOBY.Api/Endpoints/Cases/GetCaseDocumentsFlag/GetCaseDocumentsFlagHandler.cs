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
        // ToDo ask how validate if case exist efficiently  

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
        .Where(f => f.eACodeMainObj is not null && f.eACodeMainObj.IsVisibleForKb);

        return null;
    }
}
