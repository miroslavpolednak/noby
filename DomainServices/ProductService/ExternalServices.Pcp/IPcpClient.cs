using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.ProductService.ExternalServices.Pcp;

public interface IPcpClient
    : IExternalServiceClient
{
    Task<string> CreateProduct(long caseId, long customerKbId, string pcpProductIdOrObjectCode, CancellationToken cancellationToken = default);

	Task<string> UpdateProduct(string pcpId, List<long> customersKbIds, CancellationToken cancellationToken = default);

	const string ServiceName = "Pcp";

    const string Version = "V1";
    const string Version2 = "V2";
}
