using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

internal sealed class MockSbWebApiClient
    : ISbWebApiClient
{
    public Task<IServiceCallResult> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult());
    }
}
