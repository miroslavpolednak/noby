namespace ExternalServices.SbWebApi.V1;

public interface ISbWebApiClient
{
    Task<IServiceCallResult> CaseStateChanged(Shared.CaseStateChangedRequest request, CancellationToken cancellationToken);
}
