namespace ExternalServices.Sulm.V1;

public interface ISulmClient
{
    Task<IServiceCallResult> StopUse(long partyId, string usageCode, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> StartUse(long partyId, string usageCode, CancellationToken cancellationToken = default(CancellationToken));
}
