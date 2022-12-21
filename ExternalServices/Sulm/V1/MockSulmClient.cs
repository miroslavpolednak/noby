namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient 
    : ISulmClient
{
    public Task StopUse(long partyId, string usageCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartUse(long partyId, string usageCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
