using ExternalServices.SbWebApi.Shared;

namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient
{
    Task<IServiceCallResult> CaseStateChanged(CaseStateChangedModel request, CancellationToken cancellationToken);
}
