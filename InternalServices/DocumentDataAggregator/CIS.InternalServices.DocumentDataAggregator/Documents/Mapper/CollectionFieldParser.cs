using System.Collections;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Document;
using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal class CollectionFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, DocumentSourceField> sourceFields, AggregatedData aggregatedData)
    {
        if (MapperHelper.GetValue(aggregatedData, sourceFields.Key) is not IEnumerable collection)
            throw new InvalidOperationException();

        return collection.Cast<object>().SelectMany((value, index) => GetCollectionValues(value, index, sourceFields));
    }

    private static IEnumerable<DocumentMapper.SourceFieldData> GetCollectionValues(object value, int index, IEnumerable<DocumentSourceField> sourceFields) =>
        sourceFields.Select(sourceField => new DocumentMapper.SourceFieldData
        {
            SourceFieldId = sourceField.SourceFieldId,
            AcroFieldName = sourceField.AcroFieldName + (index + 1),
            StringFormat = sourceField.StringFormat,
            Value = MapperHelper.GetValue(value, CollectionPathHelper.GetCollectionMemberPath(sourceField.FieldPath))
        });
}