using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.CaseStateChanged;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.CreateTask;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

internal sealed class MockSbWebApiClient
    : ISbWebApiClient
{
    public Task<CaseStateChangedResponse> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(new CaseStateChangedResponse() { RequestId = 1 });
    }

    public Task<FindTasksResponse> FindTasksByCaseId(FindByCaseIdRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new FindTasksResponse { ItemsFound = 0, Tasks = new List<IReadOnlyDictionary<string, string>>() });
    }

    public Task<FindTasksResponse> FindTasksByTaskId(FindByTaskIdRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new FindTasksResponse { ItemsFound = 0, Tasks = new List<IReadOnlyDictionary<string, string>>() });
    }

    public Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(new Dto.CreateTask.CreateTaskResponse
        {
            TaskIdSB = 1,
            TaskId = 1
        });
    }

    public Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }
}
