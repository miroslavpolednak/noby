using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CodebookService.ExternalServices.RDM.V1;

public interface IRDMClient
    : IExternalServiceClient
{
    const string Version = "V1";
}
