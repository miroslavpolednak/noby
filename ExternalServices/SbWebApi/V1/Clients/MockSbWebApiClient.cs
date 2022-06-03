using ExternalServices.SbWebApi.Shared;

namespace ExternalServices.SbWebApi.V1;

internal sealed class MockSbWebApiClient
    : ISbWebApiClient
{
    public Task<IServiceCallResult> CaseStateChanged(CaseStateChangedModel request, CancellationToken cancellationToken)
    {
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult());
    }

    public Task<IServiceCallResult> InputRequest(long caseId, string riskBusinessCaseId, CancellationToken cancellationToken)
    {
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult());
    }
}
