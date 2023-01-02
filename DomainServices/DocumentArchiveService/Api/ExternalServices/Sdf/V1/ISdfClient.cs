using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Sdf.V1;

internal interface ISdfClient : IExternalServiceClient
{
    const string Version = "V1";

    Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalIdSdfQuery query, CancellationToken cancellationToken);

    Task<FindDocumentsOutput> FindDocuments(FindSdfDocumentsQuery query, CancellationToken cancellationToken);
}
