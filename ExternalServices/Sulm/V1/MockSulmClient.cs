namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient : ISulmClient
{
    public Versions Version { get; } = Versions.V1;

    public Task<IServiceCallResult> StopUse(long partyId, string usageCode, CancellationToken cancellationToken)
    {
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult());
    }

    public Task<IServiceCallResult> StartUse(long partyId, string usageCode, CancellationToken cancellationToken)
    {
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult());
    }
}
