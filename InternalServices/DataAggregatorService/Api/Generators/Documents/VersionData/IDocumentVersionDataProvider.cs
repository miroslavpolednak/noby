using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.VersionData;

internal interface IDocumentVersionDataProvider
{
    Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken);
}