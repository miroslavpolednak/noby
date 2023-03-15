using DomainServices.CaseService.ExternalServices.SbWebApi.Dto;

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1;

internal sealed class MockSbWebApiClient
    : ISbWebApiClient
{
    public Task<CaseStateChangedResponse> CaseStateChanged(CaseStateChangedRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(new CaseStateChangedResponse() { RequestId = 1 });
    }
}
