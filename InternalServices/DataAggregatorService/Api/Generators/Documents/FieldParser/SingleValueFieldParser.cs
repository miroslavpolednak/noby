﻿using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.FieldParser;

internal class SingleValueFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentSourceFieldData> GetFields(IEnumerable<DocumentSourceField> sourceFields, AggregatedData aggregatedData) =>
        sourceFields.Select(field => CreateMapperField(field, aggregatedData));

    private static DocumentSourceFieldData CreateMapperField(DocumentSourceField sourceField, AggregatedData aggregatedData)
    {
        var value = MapperHelper.GetValue(aggregatedData, sourceField.FieldPath);

        if (value is string str && string.IsNullOrWhiteSpace(str))
            value = null;

        return new DocumentSourceFieldData
        {
            AcroFieldName = sourceField.AcroFieldName,
            StringFormat = GetStringFormat(sourceField, value),
            Value = value ?? sourceField.DefaultTextIfNull,
            TextAlign = sourceField.TextAlign,
            VAlign = sourceField.VAlign,
            DefaultValueWasUsed = value is null && sourceField.DefaultTextIfNull is not null
        };
    }

    private static string? GetStringFormat(DocumentSourceField sourceField, object? value)
    {
        if (value is null && sourceField.DefaultTextIfNull is not null)
            return default;

        return sourceField.StringFormat;
    }
}