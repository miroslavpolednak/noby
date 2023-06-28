using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

internal interface IDocumentVersionDataProvider
{
    Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken);
}