using CIS.Core.Security;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Infrastructure.Services.DocumentHelper;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler : IRequestHandler<GetDocumentListRequest, GetDocumentListResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly IDocumentHelperService _documentHelper;

    public GetDocumentListHandler(
            IDocumentArchiveServiceClient client,
            ICurrentUserAccessor currentUserAccessor,
            ICodebookServiceClient codebookServiceClient,
            IDocumentHelperService documentHelper)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
        _codebookServiceClient = codebookServiceClient;
        _documentHelper = documentHelper;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var getDocumentListResult = await _client.GetDocumentList(new()
        {
            CaseId = request.CaseId,
            FormId = request.FormId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),

        }, cancellationToken);

        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _client.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = _documentHelper.MapGetDocumentListMetadata(getDocumentListResult);
        var documentInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var mergedDocumentMetadata = _documentHelper.MergeDocuments(documentListMetadata, documentInQueueMetadata);
        var mergedDocumentMetadataFiltered = (await _documentHelper.FilterDocumentsVisibleForKb(mergedDocumentMetadata, cancellationToken)).ToList();

        var finalResponse = new GetDocumentListResponse();
        finalResponse.DocumentsMetadata = mergedDocumentMetadataFiltered;
        finalResponse.CategoryEaCodeMain = await _documentHelper.CalculateCategoryEaCodeMain(mergedDocumentMetadataFiltered, cancellationToken);
        return finalResponse;
    }
}
