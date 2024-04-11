using ExternalServices.SbWebApi.Dto.CaseStateChanged;
using ExternalServices.SbWebApi.Dto.CompleteTask;
using ExternalServices.SbWebApi.Dto.CreateTask;
using ExternalServices.SbWebApi.Dto.FindTasks;
using ExternalServices.SbWebApi.Dto.Refinancing;
using ExternalServices.SbWebApi.Dto.UpdateTask;

namespace ExternalServices.SbWebApi.V1;

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

    public Task UpdateTask(UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(decimal InterestRate, int? NewFixationTime)> GetRefixationInterestRate(long caseId, DateTime date, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<string?> ISbWebApiClient.GenerateRetentionDocument(GenerateRetentionDocumentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GenerateRefixationDocument(GenerateRefixationDocumentRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GenerateInterestRateNotificationDocument(GenerateInterestRateNotificationDocumentRequest documentRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
