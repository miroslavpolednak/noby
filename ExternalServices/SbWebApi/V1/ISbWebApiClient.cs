using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient
{
    /// <summary>
    /// Notifikace SB o změně stavu Case
    /// </summary>
    Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
