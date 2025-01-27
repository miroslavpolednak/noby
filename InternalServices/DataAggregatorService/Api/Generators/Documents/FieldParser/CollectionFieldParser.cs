﻿using System.Collections;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.FieldParser;

internal class CollectionFieldParser : ISourceFieldParser
{
    private readonly string _collectionPath;

    public CollectionFieldParser(string collectionPath)
    {
        _collectionPath = collectionPath;
    }

    public IEnumerable<DocumentSourceFieldData> GetFields(IEnumerable<DocumentSourceField> sourceFields, AggregatedData aggregatedData)
    {
        if (MapperHelper.GetValue(aggregatedData, _collectionPath) is not IEnumerable collection)
            throw new InvalidOperationException($"Path '{_collectionPath}' does not return IEnumerable.");

        return collection.Cast<object>().SelectMany((value, index) => GetCollectionValues(value, index, sourceFields));
    }

    private static IEnumerable<DocumentSourceFieldData> GetCollectionValues(object value, int index, IEnumerable<DocumentSourceField> sourceFields) =>
        sourceFields.Select(sourceField =>
        {
            var fieldPath = CollectionPathHelper.GetCollectionMemberPath(sourceField.FieldPath);

            return new DocumentSourceFieldData
            {
                AcroFieldName = sourceField.AcroFieldName + (index + 1),
                StringFormat = sourceField.StringFormat,
                Value = string.Empty.Equals(fieldPath, StringComparison.OrdinalIgnoreCase) ? value : MapperHelper.GetValue(value, fieldPath),
                TextAlign = sourceField.TextAlign,
                VAlign = sourceField.VAlign
            };
        });
}