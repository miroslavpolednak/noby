using NOBY.Api.Endpoints.Cases.Dto;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal sealed class GetTaskListHandler : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    private readonly WorkflowMapper _mapper;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetTaskListHandler(
        WorkflowMapper mapper,
        DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _mapper = mapper;
        _codebookService = codebookService;
        _caseService = caseService;
    }

    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var workflowTasks = LoadWorkflowTasks(request.CaseId, cancellationToken);
        var workflowProcesses = LoadWorkflowProcesses(request.CaseId, cancellationToken);

        return new GetTaskListResponse
        {
            Tasks = await workflowTasks,
            Processes = await workflowProcesses
        };
    }

    private async Task<List<Dto.WorkflowTask>?> LoadWorkflowTasks(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);
        
        if (!tasks.Any())
            return default;

        return await _mapper.Map(tasks, cancellationToken);
    }

    private async Task<List<WorkflowProcess>> LoadWorkflowProcesses(long caseId, CancellationToken cancellationToken)
    {
        var processes = await _caseService.GetProcessList(caseId, cancellationToken);
        var processStates = await _codebookService.WorkflowProcessStatesNoby(cancellationToken);

        return processes.Select(p => new WorkflowProcess
        {
            ProcessId = p.ProcessId,
            CreatedOn = p.CreatedOn,
            ProcessNameLong = p.ProcessNameLong,
            StateName = p.StateName,
            StateIndicator = GetStateIndicator(p.StateName)
        }).ToList();

        StateIndicator GetStateIndicator(string stateName) => Enum.Parse<StateIndicator>(processStates.First(s => s.Name.Equals(stateName, StringComparison.InvariantCultureIgnoreCase)).Indicator);
    }
}
