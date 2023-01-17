using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.FieldParser;

internal class SingleValueFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentSourceFieldData> GetFields(IEnumerable<DocumentSourceField> sourceFields, AggregatedData aggregatedData) =>
        sourceFields.Select(field => CreateMapperField(field, aggregatedData));

    private static DocumentSourceFieldData CreateMapperField(DocumentSourceField sourceField, AggregatedData aggregatedData)
    {
        var value = MapperHelper.GetValue(aggregatedData, sourceField.FieldPath);

        return new DocumentSourceFieldData
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