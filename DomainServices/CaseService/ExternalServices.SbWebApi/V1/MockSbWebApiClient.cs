using DomainServices.CaseService.ExternalServices.SbWebApi.Dto;
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

    public Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByCaseId(FindByCaseIdRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<IReadOnlyDictionary<string, string>>>(new List<IReadOnlyDictionary<string, string>>());
    }

    public Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByTaskId(FindByTaskIdRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<IReadOnlyDictionary<string, string>>>(new List<IReadOnlyDictionary<string, string>>());
    }

    public Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByContractNumber(Dto.FindTasks.FindByContractNumberRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<IReadOnlyDictionary<string, string>>>(new List<IReadOnlyDictionary<string, string>>());
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

    public Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
