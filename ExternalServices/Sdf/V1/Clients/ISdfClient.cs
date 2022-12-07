using ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace ExternalServices.Sdf.V1.Clients
{
    public interface ISdfClient
    {
        Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalIdSdfQuery query, CancellationToken cancellationToken);

        Task<FindDocumentsOutput> FindDocuments(FindSdfDocumentsQuery query, CancellationToken cancellationToken);
    }
}
