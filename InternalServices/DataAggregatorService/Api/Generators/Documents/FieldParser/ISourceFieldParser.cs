using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.FieldParser;

internal interface ISourceFieldParser
{
    public static ISourceFieldParser Create(string collectionPath) =>
        string.IsNullOrWhiteSpace(collectionPath) ? new SingleValueFieldParser() : new CollectionFieldParser(collectionPath);

    IEnumerable<DocumentSourceFieldData> GetFields(IEnumerable<DocumentSourceField> sourceFields, AggregatedData aggregatedData);
}