using System.Collections;
using System.ComponentModel;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal class DocumentMapper
{
    private const string FieldPathCollectionMarker = "[]";

    private readonly DocumentConfiguration _config;
    private readonly AggregatedData _aggregatedData;

    private DocumentMapper(DocumentConfiguration config, AggregatedData aggregatedData)
    {
        _config = config;
        _aggregatedData = aggregatedData;
    }

    public static DocumentMapper CreateInstance(DocumentConfiguration config, AggregatedData aggregatedData) => new(config, aggregatedData);

    public IReadOnlyCollection<DocumentFieldData> GetDocumentFields()
    {
        var dynamicStringFormats = GetDynamicStringFormats(_config.DynamicStringFormats);

        var sourceFields = GetSourceFields();

        return sourceFields.Select(ParseDocumentFieldData).ToList();

        DocumentFieldData ParseDocumentFieldData(SourceFieldData fieldData)
        {
            var stringFormat = fieldData.StringFormat;

            if (fieldData.SourceFieldId.HasValue && dynamicStringFormats.ContainsKey(fieldData.SourceFieldId.Value))
                stringFormat = dynamicStringFormats[fieldData.SourceFieldId.Value];

            return new DocumentFieldData
            {
                FieldName = fieldData.TemplateFieldName,
                Value = fieldData.Value!,
                StringFormat = stringFormat
            };
        }
    }

    private IEnumerable<SourceFieldData> GetSourceFields() =>
        _config.SourceFields
               .Select(f => new SourceFieldData
               {
                   SourceFieldId = f.SourceFieldId,
                   TemplateFieldName = f.TemplateFieldName,
                   StringFormat = f.StringFormat,
                   Value = MapperHelper.GetValue(_aggregatedData, f.FieldPath)
               })
               .Where(f => f.Value switch
               {
                   null => false,
                   string text => !string.IsNullOrWhiteSpace(text),
                   _ => true
               });

    private Dictionary<int, string> GetDynamicStringFormats(ILookup<int, DocumentDynamicStringFormat> dynamicStringFormats) =>
        dynamicStringFormats.Select(x => new
                            {
                                x.Key,
                                Format = x.OrderBy(c => c.Priority)
                                          .Where(c => ValidateCondition(c.Conditions))
                                          .Select(c => c.Format)
                                          .FirstOrDefault()
                            })
                            .Where(c => c.Format is not null)
                            .ToDictionary(x => x.Key, v => v.Format!);

    private bool ValidateCondition(ICollection<DocumentDynamicStringFormatCondition> conditions) =>
        conditions.GroupBy(c =>
                  {
                      var splitPath = c.SourceFieldPath.Split(FieldPathCollectionMarker);

                      return splitPath.Length == 1 ? string.Empty : splitPath.First();
                  })
                  .All(group => group.Key == string.Empty ? group.All(ValidateConditionValue) : ValidateConditionCollection(group));

    private bool ValidateConditionValue(DocumentDynamicStringFormatCondition condition)
    {
        var value = MapperHelper.GetValue(_aggregatedData, condition.SourceFieldPath);

        return CompareValueToStringValue(value, condition.EqualToValue);
    }

    private bool ValidateConditionCollection(IGrouping<string, DocumentDynamicStringFormatCondition> groupedConditions)
    {
        if (MapperHelper.GetValue(_aggregatedData, groupedConditions.Key) is not IEnumerable collection)
            throw new InvalidOperationException();

        return collection.Cast<object>().Any(obj =>
        {
            return groupedConditions.All(condition =>
            {
                var collectionValuePath = condition.SourceFieldPath.Split(FieldPathCollectionMarker).Last().TrimStart('.');
                var value = MapperHelper.GetValue(obj, collectionValuePath);

                return CompareValueToStringValue(value, condition.EqualToValue);
            });
        });
    }

    private static bool CompareValueToStringValue(object? value, string? stringValue)
    {
        if (value is null)
            return Equals(default, stringValue);

        if (stringValue is null)
            return false;

        var converter = TypeDescriptor.GetConverter(value.GetType());

        return Equals(value, converter.ConvertFromString(stringValue));
    }

    public class SourceFieldData
    {
        public int? SourceFieldId { get; init; }
        public string TemplateFieldName { get; init; } = null!;
        public string? StringFormat { get; init; }
        public object? Value { get; init; }
    }
}