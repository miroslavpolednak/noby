using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1.Model;

namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1;

public interface IDocumentServiceRepository : IExternalServiceClient
{
    const string Version = "V1";

    Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<DocumentServiceQueryResult>> FindTcpDocument(FindTcpDocumentQuery query, CancellationToken cancellationToken);
}
