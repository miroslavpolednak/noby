using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbWebApi.Dto.CompleteTask;
using ExternalServices.SbWebApi.Dto.Refinancing;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task<Dto.CaseStateChanged.CaseStateChangedResponse> CaseStateChanged(Dto.CaseStateChanged.CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Zašle odpověď do workflow úkolu Dožádání.
    /// </summary>
    /// <returns>Kód, který se vrátil z SB</returns>
    Task CompleteTask(CompleteTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání seznamu úkolů a podúkolů ze SB podle Case ID.
    /// </summary>
    Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByCaseId(Dto.FindTasks.FindByCaseIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání podúkolů ze SB podle Task SB-ID.
    /// </summary>
    Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByTaskId(Dto.FindTasks.FindByTaskIdRequest request, CancellationToken cancellationToken = default);

    Task<IList<IReadOnlyDictionary<string, string>>> FindTasksByContractNumber(Dto.FindTasks.FindByContractNumberRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// založení workflow úkolu
    /// </summary>
    Task<Dto.CreateTask.CreateTaskResponse> CreateTask(Dto.CreateTask.CreateTaskRequest request, CancellationToken cancellationToken = default(CancellationToken));

    Task CancelTask(int taskIdSB, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdateTask(Dto.UpdateTask.UpdateTaskRequest request, CancellationToken cancellationToken = default);

    Task<(decimal InterestRate, int? NewFixationTime)> GetRefixationInterestRate(long caseId, DateTime interestRateValidTo, CancellationToken cancellationToken);

    Task<string?> GenerateRetentionDocument(GenerateRetentionDocumentRequest request, CancellationToken cancellationToken);
    Task<string?> GenerateRefixationDocument(GenerateRefixationDocumentRequest request, CancellationToken cancellationToken = default);
    Task<string?> GenerateInterestRateNotificationDocument(GenerateInterestRateNotificationDocumentRequest documentRequest, CancellationToken cancellationToken = default);
}
