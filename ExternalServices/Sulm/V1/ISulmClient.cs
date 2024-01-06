namespace ExternalServices.Sulm.V1;

public interface ISulmClient
    : IExternalServiceClient
{
    Task StopUse(long kbCustomerId, IList<SharedTypes.Types.UserIdentity> identities, string purposeCode, CancellationToken cancellationToken = default(CancellationToken));

    Task StartUse(long kbCustomerId, IList<SharedTypes.Types.UserIdentity> identities, string purposeCode, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";

    public const string PurposeMLAP = "MLAP";
    public const string PurposeMPAP = "MPAP";
    public const string PurposeMLAX = "MLAX";

    internal static string GetChannelCode(IList<SharedTypes.Types.UserIdentity>? identities)
    {
        return identities?.Any(t => t.Scheme == SharedTypes.Enums.UserIdentitySchemes.BrokerId) ?? false
            ? "CH0001" : "CH0002";
    }
}
