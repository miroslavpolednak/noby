using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServicesTcp.V1.Model;

namespace ExternalServicesTcp.V1.Repositories;

public interface IDocumentServiceRepository: IExternalServiceClient
{
    const string Version = "V1";

    Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<DocumentServiceQueryResult>> FindTcpDocument(FindTcpDocumentQuery query, CancellationToken cancellationToken);
}
