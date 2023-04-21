using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task<Dto.CaseStateChanged.CaseStateChangedResponse> CaseStateChanged(Dto.CaseStateChanged.CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání seznamu úkolů a podúkolů ze SB podle Case ID.
    /// </summary>
    Task<Dto.FindTasks.FindTasksResponse> FindTasksByCaseId(Dto.FindTasks.FindByCaseIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání podúkolů ze SB podle Task SB-ID.
    /// </summary>
    Task<Dto.FindTasks.FindTasksResponse> FindTasksByTaskId(Dto.FindTasks.FindByTaskIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// založení workflow úkolu
    /// </summary>
    Task<Dto.CreateTask.CreateTaskResponse> CreateTask(Dto.CreateTask.CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken));

    Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken));
}
