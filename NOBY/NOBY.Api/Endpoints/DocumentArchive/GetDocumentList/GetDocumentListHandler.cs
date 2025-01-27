﻿using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Services.DocumentHelper;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler(
    IDocumentArchiveServiceClient _client,
    ICurrentUserAccessor _currentUserAccessor,
    IDocumentHelperServiceOld _documentHelper) 
    : IRequestHandler<GetDocumentListRequest, DocumentArchiveGetDocumentListResponse>
{
    public async Task<DocumentArchiveGetDocumentListResponse> Handle(GetDocumentListRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var getDocumentListResult = await _client.GetDocumentList(new()
        {
            CaseId = request.CaseId,
            FormId = request.FormId,
            UserLogin = user is null ? "Unknown NOBY user" : user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),

        }, cancellationToken);

        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _client.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = _documentHelper.MapGetDocumentListMetadata(getDocumentListResult);
        var documentInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var mergedDocumentMetadata = _documentHelper.MergeDocuments(documentListMetadata, documentInQueueMetadata);

        if (!string.IsNullOrWhiteSpace(request.FormId))
            mergedDocumentMetadata = mergedDocumentMetadata.Where(d => d.FormId == request.FormId);

        var mergedDocumentMetadataFiltered = (await _documentHelper.FilterDocumentsVisibleForKb(mergedDocumentMetadata, cancellationToken)).ToList();

        var finalResponse = new DocumentArchiveGetDocumentListResponse();
        finalResponse.DocumentsMetadata = mergedDocumentMetadataFiltered;
        finalResponse.CategoryEaCodeMain = await _documentHelper.CalculateCategoryEaCodeMain(mergedDocumentMetadataFiltered, cancellationToken);
        return finalResponse;
    }
}
