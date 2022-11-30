using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : IExternalServiceClient
{
    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";

    public new string GetVersion() => "V1";
}
