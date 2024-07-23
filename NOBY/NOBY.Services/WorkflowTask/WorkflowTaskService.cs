using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CaseService.Clients.v1;
using DomainServices.DocumentArchiveService.Clients;
using NOBY.Services.DocumentHelper;
using NOBY.Services.WorkflowMapper;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.ApiContracts;

namespace NOBY.Services.WorkflowTask;

[TransientService, AsImplementedInterfacesService]
internal sealed class WorkflowTaskService(
        IWorkflowMapperService _mapper,
        ICaseServiceClient _caseService,
        IDocumentArchiveServiceClient _documentArchiveService,
        IDocumentHelperService _documentHelper)
        : IWorkflowTaskService
{
    public async Task<(SharedTypesWorkflowTask Task, SharedTypesWorkflowTaskDetail TaskDetail, List<SharedTypesDocumentsMetadata> Documents)> GetTaskDetail(
        long caseId, 
        int taskIdSb, 
        CancellationToken cancellationToken = default)
    {
        var taskDetails = await _caseService.GetTaskDetail(taskIdSb, cancellationToken);
        var taskDetail = taskDetails.TaskDetail
            ?? throw new NobyValidationException($"TaskDetail for Task {taskIdSb} not found.");

        var taskDto = await _mapper.MapTask(taskDetails.TaskObject, cancellationToken);
        var taskDetailDto = await _mapper.MapTaskDetail(taskDetails.TaskObject, taskDetail, cancellationToken);

        if ((taskDetail.TaskDocumentIds?.Any() ?? false) && taskDto.TaskTypeId != 6)
        {
            var finalDocs = await getDocuments(caseId, taskDetail.TaskDocumentIds.ToArray(), cancellationToken);
            return (taskDto, taskDetailDto, finalDocs);
        }
        else
        {
            return (taskDto, taskDetailDto, new List<SharedTypesDocumentsMetadata>());
        }
    }

    private async Task<List<SharedTypesDocumentsMetadata>> getDocuments(long caseId, string[] taskDocumentIds, CancellationToken cancellationToken)
    {
        var documentListResponse = await _documentArchiveService.GetDocumentList(new()
        {
            CaseId = caseId
        }, cancellationToken);

        // Get doc from queue 
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = caseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange([100, 110, 200, 300]);
        var getDocumentsInQueueResult = await _documentArchiveService.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var documentListMetadata = _documentHelper.MapGetDocumentListMetadata(documentListResponse);
        var documentInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var mergedDocumentMetadata = _documentHelper.MergeDocuments(documentListMetadata, documentInQueueMetadata);
        var mergedDocumentMetadataFiltered = await _documentHelper.FilterDocumentsVisibleForKb(mergedDocumentMetadata, cancellationToken);

        return mergedDocumentMetadataFiltered
            .Where(m => taskDocumentIds.Contains(m.DocumentId))
            .ToList();
    }
}
