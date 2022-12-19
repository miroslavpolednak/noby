using System.Collections;
using System.ComponentModel;
using CIS.InternalServices.DataAggregator.Configuration.Document;
using CIS.InternalServices.DataAggregator.DataServices;
using CIS.InternalServices.DataAggregator.Helpers;

namespace CIS.InternalServices.DataAggregator.Documents.Mapper;

internal class DynamicStringFormatParser
{
    private readonly AggregatedData _aggregatedData;

    private DynamicStringFormatParser(AggregatedData aggregatedData)
    {
        _aggregatedData = aggregatedData;
    }

    public static string? ParseStringFormat(IGrouping<int, DocumentDynamicStringFormat> dynamicStringFormats, AggregatedData aggregatedData)
    {
        var parser = new DynamicStringFormatParser(aggregatedData);

        return dynamicStringFormats.OrderBy(d => d.Priority)
                                   .Where(d => parser.ValidateCondition(d.Conditions))
                                   .Select(d => d.Format).FirstOrDefault();
    }

    private bool ValidateCondition(IEnumerable<DocumentDynamicStringFormatCondition> conditions) =>
        conditions.GroupBy(c => CollectionPathHelper.GetCollectionPath(c.SourceFieldPath))
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
                var value = MapperHelper.GetValue(obj, CollectionPathHelper.GetCollectionMemberPath(condition.SourceFieldPath));

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

        if (converter.GetType() != typeof(TypeConverter))
            return Equals(value, converter.ConvertFromString(stringValue));

        //Probly GrpcDecimal etc
        value = MapperHelper.ConvertObjectImplicitly(value);
        converter = TypeDescriptor.GetConverter(value.GetType());

        return Equals(value, converter.ConvertFromString(stringValue));
    }
}