using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Crem.V1;

public interface ICremClient
    : IExternalServiceClient
{
    Task<Dto.FlatsForAddress> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default);

    const string Version = "V1";
}
