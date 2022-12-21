using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Sulm.V1;

public interface ISulmClient
    : IExternalServiceClient
{
    Task StopUse(long partyId, string usageCode, CancellationToken cancellationToken = default(CancellationToken));

    Task StartUse(long partyId, string usageCode, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
