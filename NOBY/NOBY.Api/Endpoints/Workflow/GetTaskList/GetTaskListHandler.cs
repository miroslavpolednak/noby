using DomainServices.CaseService.Clients.v1;
using NOBY.Services.WorkflowMapper;

namespace NOBY.Api.Endpoints.Workflow.GetTaskList;

internal sealed class GetTaskListHandler(
    ICaseServiceClient _caseService, 
    IWorkflowMapperService _mapper)
    : IRequestHandler<GetTaskListRequest, WorkflowGetTaskListResponse>
{
    public async Task<WorkflowGetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var response = new WorkflowGetTaskListResponse();

        var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(t => _allowedTaskTypes.Contains(t.TaskTypeId))
            .ToList();
        foreach (var task in tasks)
        {
            response.Tasks.Add(await _mapper.MapTask(task, cancellationToken));
        }

        var processes = (await _caseService.GetProcessList(request.CaseId, cancellationToken))
            .Where(t => _allowedProcessTypes.Contains(t.ProcessTypeId))
            .ToList();
        response.Processes = processes.Select(p => _mapper.MapProcess(p)).ToList();

        return response;
    }

    private static readonly int[] _allowedProcessTypes =
        [
            (int)WorkflowProcesses.Main,
            (int)WorkflowProcesses.Change,
            (int)WorkflowProcesses.Refinancing,
            (int)WorkflowProcesses.ExtraPayment
        ];
    
    private static readonly int[] _allowedTaskTypes =
        [
            (int)WorkflowTaskTypes.Dozadani,
            (int)WorkflowTaskTypes.PriceException,
            (int)WorkflowTaskTypes.Consultation,
            (int)WorkflowTaskTypes.Signing,
            (int)WorkflowTaskTypes.PredaniNaSpecialitu
        ];
}
