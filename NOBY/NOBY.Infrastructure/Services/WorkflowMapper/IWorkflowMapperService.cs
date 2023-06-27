using _Case = DomainServices.CaseService.Contracts;
using _Dto = NOBY.Dto.Workflow;

namespace NOBY.Infrastructure.Services.WorkflowMapper;

public interface IWorkflowMapperService
{
    _Dto.WorkflowProcess MapProcess(_Case.ProcessTask task);

    Task<_Dto.WorkflowTask> MapTask(_Case.WorkflowTask task, CancellationToken cancellationToken = default);

    Task<_Dto.WorkflowTaskDetail> MapTaskDetail(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem, CancellationToken cancellationToken = default);
}
