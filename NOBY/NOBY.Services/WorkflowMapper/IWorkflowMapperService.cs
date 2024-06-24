using NOBY.ApiContracts;
using _Case = DomainServices.CaseService.Contracts;

namespace NOBY.Services.WorkflowMapper;

public interface IWorkflowMapperService
{
    SharedTypesWorkflowProcess MapProcess(_Case.ProcessTask task);

    Task<SharedTypesWorkflowTask> MapTask(_Case.WorkflowTask task, CancellationToken cancellationToken = default);

    Task<SharedTypesWorkflowTaskDetail> MapTaskDetail(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem, CancellationToken cancellationToken = default);
}
