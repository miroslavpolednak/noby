using CIS.InternalServices.DocumentDataAggregator.Configuration.Document;
using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal class SingleValueFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, DocumentSourceField> sourceFields, AggregatedData aggregatedData) =>
        sourceFields.Select(field => CreateMapperField(field, aggregatedData));

    private static DocumentMapper.SourceFieldData CreateMapperField(DocumentSourceField sourceField, AggregatedData aggregatedData)
    {
        var value = MapperHelper.GetValue(aggregatedData, sourceField.FieldPath);

        return new DocumentMapper.SourceFieldData
        {
            SourceFieldId = sourceField.SourceFieldId,
            AcroFieldName = sourceField.AcroFieldName,
            StringFormat = GetStringFormat(sourceField, value),
            Value = value ?? sourceField.DefaultTextIfNull
        };
    }

    private static string? GetStringFormat(DocumentSourceField sourceField, object? value)
    {
        if (value is null && sourceField.DefaultTextIfNull is not null)
            return default;

        return sourceField.StringFormat;
    }
}