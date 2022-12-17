using System.Collections;
using CIS.InternalServices.DataAggregator.Configuration.Document;
using CIS.InternalServices.DataAggregator.DataServices;
using CIS.InternalServices.DataAggregator.Helpers;

namespace CIS.InternalServices.DataAggregator.Documents.Mapper;

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