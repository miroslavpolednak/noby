using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : IExternalServiceClient
{
    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task<CaseStateChangedResponse> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
