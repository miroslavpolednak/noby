namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient 
    : ISulmClient
{
    public Task StopUse(long kbUserId, string channgelCode, string purposeCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartUse(long kbUserId, string channgelCode, string purposeCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
