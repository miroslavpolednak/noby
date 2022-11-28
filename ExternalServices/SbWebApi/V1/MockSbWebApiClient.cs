using ExternalServices.SbWebApi.Dto;

namespace ExternalServices.SbWebApi.V1;

internal sealed class MockSbWebApiClient
    : ISbWebApiClient
{
    public Task CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
