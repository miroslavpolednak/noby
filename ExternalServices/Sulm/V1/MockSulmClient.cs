namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient 
    : ISulmClient
{
    public Task StopUse(long kbCustomerId, IList<CIS.Foms.Types.UserIdentity> userIdentities, string purposeCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartUse(long kbCustomerId, IList<CIS.Foms.Types.UserIdentity> userIdentities, string purposeCode, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
