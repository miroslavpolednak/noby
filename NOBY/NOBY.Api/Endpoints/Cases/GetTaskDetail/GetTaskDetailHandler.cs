using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Api.Endpoints.Cases.Dto;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

internal sealed class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly WorkflowMapper _mapper;
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentHelper _documentHelper;

    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var task = taskList.FirstOrDefault(t => t.TaskId == request.TaskId)
            ?? throw new CisNotFoundException(90001, $"Task {request.TaskId} not found.");

        var taskDetails = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);
        var taskDetail = taskDetails.TaskDetails.FirstOrDefault(t => t.TaskObject.TaskId == task.TaskId)?.TaskDetail
            ?? throw new CisNotFoundException(90001, $"TaskDetail for Task {request.TaskId} not found.");

        var documentIds = taskDetail.TaskDocumentIds.ToHashSet();

        var documentListResponse = await _documentArchiveService.GetDocumentList(new DomainServices.DocumentArchiveService.Contracts.GetDocumentListRequest
        {
            CaseId = request.CaseId
        }, cancellationToken);

        // Get doc from queue 
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = request.CaseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _documentArchiveService.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = _documentHelper.MapGetDocumentListMetadata(documentListResponse);
        var documentInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var mergedDocumentMetadata = _documentHelper.MergeDocuments(documentListMetadata, documentInQueueMetadata);
        var mergedDocumentMetadataFiltered = await _documentHelper.FilterDocumentsVisibleForKb(mergedDocumentMetadata, cancellationToken);

        var taskDetailDto = await _mapper.Map(task, taskDetail, cancellationToken);
        var taskDto = await _mapper.Map(task, cancellationToken);

        return new GetTaskDetailResponse
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = mergedDocumentMetadataFiltered.Where(m => documentIds.Contains(m.DocumentId)).ToList()
        };
    }
    
    public GetTaskDetailHandler(
            WorkflowMapper mapper,
            ICaseServiceClient caseService,
            IDocumentArchiveServiceClient documentArchiveService,
            IDocumentHelper documentHelper)
    {
        _mapper = mapper;
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _documentHelper = documentHelper;
    }
}
