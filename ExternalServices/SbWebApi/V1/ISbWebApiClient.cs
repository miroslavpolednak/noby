using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient 
    : ISbWebApi
{
    Task<IServiceCallResult> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken);
}
