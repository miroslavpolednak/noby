using DomainServices.CaseService.Clients;
using NOBY.Api.Endpoints.Cases.Dto;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal sealed class GetTaskListHandler : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    private readonly WorkflowMapper _mapper;
    private readonly ICaseServiceClient _caseService;

    public GetTaskListHandler(WorkflowMapper mapper, ICaseServiceClient caseService)
    {
        _mapper = mapper;
        _caseService = caseService;
    }

    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var workflowTasks = LoadWorkflowTasks(request.CaseId, cancellationToken);
        var workflowProcesses = LoadWorkflowProcesses(request.CaseId, cancellationToken);

        //Temporary mock - The old answer must be used to avoid violating FE
        var newResponse = new GetTaskListResponseNew
        {
            Tasks = await workflowTasks,
            Processes = await workflowProcesses
        };

        return new GetTaskListResponse
        {
            Tasks = newResponse.Tasks?.Select(t => new WorkflowTask
            {
                TaskId = t.TaskId,
                TaskProcessId = t.ProcessId,
                Name = t.TaskTypeName,
                TypeId = t.TaskTypeId,
                CategoryId = 0,
                CreatedOn = t.CreatedOn,
                StateId = t.StateId
            }).ToList()
        };
    }

    private async Task<List<Dto.WorkflowTaskNew>?> LoadWorkflowTasks(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);
        
        if (!tasks.Any())
            return default;

        return await _mapper.Map(tasks, cancellationToken);
    }

    private async Task<List<WorkflowProcess>> LoadWorkflowProcesses(long caseId, CancellationToken cancellationToken)
    {
        var processes = await _caseService.GetProcessList(caseId, cancellationToken);

        return processes.Select(p => new WorkflowProcess
        {
            ProcessId = p.ProcessId,
            CreatedOn = p.CreatedOn,
            ProcessNameLong = p.ProcessNameLong,
            StateName = p.StateName,
            ProcessTypeId = p.ProcessTypeId,
            StateIndicator = (StateIndicators)p.StateIndicator
        }).ToList();
    }
}
