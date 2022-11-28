using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : ISbWebApi
{
    Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
