using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class RetentionData : IDocumentData
{
    int IDocumentData.Version => 1;
}
