using ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace ExternalServices.Sdf.V1.Clients
{
    public interface ISdfClient
    {
        Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalQuery query, CancellationToken cancellationToken);
    }
}
