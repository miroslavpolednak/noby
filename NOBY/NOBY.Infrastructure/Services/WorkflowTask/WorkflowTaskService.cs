using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using NOBY.Infrastructure.Services.DocumentHelper;
using NOBY.Infrastructure.Services.WorkflowMapper;
using CIS.Core.Security;
using NOBY.Infrastructure.Security;

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

        var taskDto = await _mapper.MapTask(taskDetails.TaskObject, cancellationToken);
        var taskDetailDto = await _mapper.MapTaskDetail(taskDetails.TaskObject, taskDetail, cancellationToken);

        bool docsAllowed = taskDto.TaskTypeId != 6 || _userAccessor.HasPermission(DomainServices.UserService.Clients.Authorization.UserPermissions.UC_getWflSigningAttachments);

        if ((taskDetail.TaskDocumentIds?.Any() ?? false) && docsAllowed)
        {
            var finalDocs = await getDocuments(caseId, taskDetail.TaskDocumentIds.ToArray(), cancellationToken);
            return (taskDto, taskDetailDto, finalDocs);
        }
        else
        {
            return (taskDto, taskDetailDto, null);
        }
    }

    private async Task<List<Dto.Documents.DocumentsMetadata>> getDocuments(long caseId, string[] taskDocumentIds, CancellationToken cancellationToken)
    {
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

        return mergedDocumentMetadataFiltered
            .Where(m => taskDocumentIds.Contains(m.DocumentId))
            .ToList();
    }

    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentHelperService _documentHelper;
    private readonly IWorkflowMapperService _mapper;
    private readonly ICurrentUserAccessor _userAccessor;

    public WorkflowTaskService(
            ICurrentUserAccessor userAccessor,
            IWorkflowMapperService mapper,
            ICaseServiceClient caseService,
            IDocumentArchiveServiceClient documentArchiveService,
            IDocumentHelperService documentHelper)
    {
        _userAccessor = userAccessor;
        _mapper = mapper;
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _documentHelper = documentHelper;
    }
}
