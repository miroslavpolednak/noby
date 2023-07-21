using ExternalServices.Crem.Dto;

namespace ExternalServices.Crem.V1;

internal sealed class MockCremClient
    : ICremClient
{
    public Task<FlatsForAddress> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new FlatsForAddress());
    }
}
