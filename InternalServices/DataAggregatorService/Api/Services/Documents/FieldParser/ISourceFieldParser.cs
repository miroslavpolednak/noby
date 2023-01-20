using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.FieldParser;

internal interface ISourceFieldParser
{
    public static ISourceFieldParser Create(string collectionPath) =>
        string.IsNullOrWhiteSpace(collectionPath) ? new SingleValueFieldParser() : new CollectionFieldParser(collectionPath);

    IEnumerable<DocumentSourceFieldData> GetFields(IEnumerable<DocumentSourceField> sourceFields, AggregatedData aggregatedData);
}