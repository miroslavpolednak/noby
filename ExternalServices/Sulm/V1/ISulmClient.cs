using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Sulm.V1;

public interface ISulmClient
    : IExternalServiceClient
{
    Task StopUse(IList<CIS.Foms.Types.UserIdentity> identities, string purposeCode, CancellationToken cancellationToken = default(CancellationToken));

    Task StartUse(IList<CIS.Foms.Types.UserIdentity> identities, string purposeCode, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";

    public const string PurposeMLAP = "MLAP";
    public const string PurposeMPAP = "MPAP";

    public static string GetChannelCode(IList<CIS.Foms.Types.UserIdentity> identities)
    {
        return identities.Any(t => t.Scheme == CIS.Foms.Enums.UserIdentitySchemes.BrokerId)
            ? "CH0001" : "CH0002";
    }
}
