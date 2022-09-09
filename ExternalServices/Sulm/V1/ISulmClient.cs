namespace ExternalServices.Sulm.V1;

public interface ISulmClient
{
    Task<IServiceCallResult> StopUse(long partyId, string usageCode);

    Task<IServiceCallResult> StartUse(long partyId, string usageCode);
}
