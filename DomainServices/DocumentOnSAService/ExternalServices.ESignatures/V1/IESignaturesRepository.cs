using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatures.V1;

public interface IESignaturesRepository : IExternalServiceClient
{
    const string Version = "V1";
}