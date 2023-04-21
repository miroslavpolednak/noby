using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using NOBY.Api.Endpoints.Cases.Dto;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

internal sealed class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly WorkflowMapper _mapper;
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;

    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var task = taskList.FirstOrDefault(t => t.TaskId == request.TaskId) ?? throw new CisNotFoundException(0, "TODO");

        var taskDetails = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);
        var taskDetail = taskDetails.TaskDetails.FirstOrDefault(t => t.TaskObject.TaskId == task.TaskId)?.TaskDetail ?? throw new CisNotFoundException(0, "TODO");

        var documentIds = taskDetail.TaskDocumentIds.ToHashSet();
        var documentListResponse = await _documentArchiveService.GetDocumentList(new DomainServices.DocumentArchiveService.Contracts.GetDocumentListRequest
        {
            CaseId = request.CaseId
        }, cancellationToken);

        var taskDetailDto = await _mapper.Map(task, taskDetail, cancellationToken);
        var taskDto = await _mapper.Map(task, cancellationToken);
        
        return new GetTaskDetailResponse
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documentListResponse.Metadata
                .Where(m => documentIds.Contains(m.DocumentId))
                .Select(m => new DocumentsMetadata
                {
                    DocumentId = m.DocumentId,
                    Description = m.Description,
                    CreatedOn = m.CreatedOn,
                    FileName = m.Filename,
                    UploadStatus =  UploadStatus.Ok, // TODO: https://jira.kb.cz/browse/HFICH-5592
                    EaCodeMainId = m.EaCodeMainId
                })
                .ToList()
        };
    }
    
    public GetTaskDetailHandler(
        WorkflowMapper mapper,
        ICaseServiceClient caseService,
        IDocumentArchiveServiceClient documentArchiveService)
    {
        _mapper = mapper;
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
    }
}
