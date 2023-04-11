using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task<CaseStateChangedResponse> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání seznamu úkolů a podúkolů ze SB podle Case ID.
    /// </summary>
    Task<FindTasksResponse> FindTasksByCaseId(FindByCaseIdRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání podúkolů ze SB podle Task SB-ID.
    /// </summary>
    Task<FindTasksResponse> FindTasksByTaskId(FindByTaskIdRequest request, CancellationToken cancellationToken = default);
}
