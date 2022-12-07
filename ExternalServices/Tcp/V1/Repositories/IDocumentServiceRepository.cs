using ExternalServicesTcp.V1.Model;

namespace ExternalServicesTcp.V1.Repositories
{
    public interface IDocumentServiceRepository
    {
        Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<DocumentServiceQueryResult>> FindTcpDocument(FindTcpDocumentQuery query, CancellationToken cancellationToken);
    }
}
