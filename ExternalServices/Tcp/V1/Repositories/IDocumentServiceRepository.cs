using ExternalServicesTcp.Model;

namespace ExternalServicesTcp.V1.Repositories
{
    public interface IDocumentServiceRepository
    {
        Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken);
    }
}
