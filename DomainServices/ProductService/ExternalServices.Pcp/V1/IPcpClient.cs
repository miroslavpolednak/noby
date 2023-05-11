using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

public interface IPcpClient
    : IExternalServiceClient
{
    Task<string> CreateProduct(long caseId, long customerKbId, string pcpProductId, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
