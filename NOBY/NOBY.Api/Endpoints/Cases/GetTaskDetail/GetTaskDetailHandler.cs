using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

internal sealed class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);

        var task = taskList.FirstOrDefault(t => t.TaskId == request.TaskId) 
                   ?? throw new CisNotFoundException(0, "TODO");

        // TODO
        
        var documentListResponse = await _documentArchiveService.GetDocumentList(new DomainServices.DocumentArchiveService.Contracts.GetDocumentListRequest
        {
            CaseId = request.CaseId,
            ContractNumber = "",
            CreatedOn = DateTime.Now,
            OrderId = 0,
            UserLogin = "",
            AuthorUserLogin = "",
            FolderDocumentId = "",
            PledgeAgreementNumber = ""
        }, cancellationToken);
        
        return new GetTaskDetailResponse
        {
            TaskDetail = new WorkflowTaskDetail
            {
                TaskIdSB = task.TaskIdSb,
                PerformerLogin = "",
                PerformerName = "",
                ProcessNameLong = "",
                SentToCustomer = false,
                OrderId = 0,
                TaskCommunication = new List<TaskCommunicationItem>()
            },
            Task = new WorkflowTask
            {
                TaskId = task.TaskId,
                CreatedOn = task.CreatedOn,
                TaskTypeId = task.TaskTypeId,
                TaskTypeName = task.TaskTypeName,
                TaskSubtypeName = task.TaskSubtypeName,
                ProcessId = task.ProcessId,
                ProcessNameShort = task.ProcessNameShort,
                // StateId = taskState.Id,
                // StateName = taskState.Name,
                // StateFilter = Enum.Parse<StateFilter>(taskState.Filter, true),
                // StateIndicator = Enum.Parse<StateIndicator>(taskState.Indicator, true)
            },
            Documents = documentListResponse.Metadata
                .Select(m => new DocumentsMetadata
                {
                    DocumentId = m.DocumentId,
                    Description = m.Description,
                    CreatedOn = m.CreatedOn,
                    FileName = m.Filename,
                    // UploadStatus = m.Status,
                    EaCodeMainId = m.EaCodeMainId
                })
                .ToList()
        };
    }
    
    public GetTaskDetailHandler(
        ICaseServiceClient caseService,
        IDocumentArchiveServiceClient documentArchiveService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _currentUserAccessor = currentUserAccessor;
    }
}
