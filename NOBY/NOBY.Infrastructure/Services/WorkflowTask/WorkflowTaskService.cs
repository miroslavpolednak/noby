using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using NOBY.Infrastructure.Services.DocumentHelper;
using NOBY.Infrastructure.Services.WorkflowMapper;

namespace NOBY.Infrastructure.Services.WorkflowTask;

[TransientService, AsImplementedInterfacesService]
internal sealed class WorkflowTaskService
    : IWorkflowTaskService
{
    public async Task<(Dto.Workflow.WorkflowTask? Task, Dto.Workflow.WorkflowTaskDetail? TaskDetail, List<Dto.Documents.DocumentsMetadata>? Documents)> GetTaskDetail(
        long caseId, 
        int taskIdSb, 
        CancellationToken cancellationToken = default)
    {
        var taskDetails = await _caseService.GetTaskDetail(taskIdSb, cancellationToken);
        var taskDetail = taskDetails.TaskDetail
            ?? throw new CisNotFoundException(90001, $"TaskDetail for Task {taskIdSb} not found.");
        
        var documentIds = taskDetail.TaskDocumentIds.ToHashSet();

        var documentListResponse = await _documentArchiveService.GetDocumentList(new()
        {
            CaseId = caseId
        }, cancellationToken);

        // Get doc from queue 
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = caseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _documentArchiveService.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = _documentHelper.MapGetDocumentListMetadata(documentListResponse);
        var documentInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var mergedDocumentMetadata = _documentHelper.MergeDocuments(documentListMetadata, documentInQueueMetadata);
        var mergedDocumentMetadataFiltered = await _documentHelper.FilterDocumentsVisibleForKb(mergedDocumentMetadata, cancellationToken);

        var taskDetailDto = await _mapper.Map(taskDetails.TaskObject, taskDetail, cancellationToken);
        var taskDto = await _mapper.Map(taskDetails.TaskObject, cancellationToken);

        return (taskDto, taskDetailDto, mergedDocumentMetadataFiltered.Where(m => documentIds.Contains(m.DocumentId)).ToList());
    }

    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentHelperService _documentHelper;
    private readonly WorkflowMapperService _mapper;

    public WorkflowTaskService(
            WorkflowMapperService mapper,
            ICaseServiceClient caseService,
            IDocumentArchiveServiceClient documentArchiveService,
            IDocumentHelperService documentHelper)
    {
        _mapper = mapper;
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _documentHelper = documentHelper;
    }
}
