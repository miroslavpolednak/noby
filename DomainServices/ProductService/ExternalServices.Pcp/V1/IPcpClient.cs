using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

public interface IPcpClient
    : IExternalServiceClient
{
    Task x();

    const string Version = "V1";
}
