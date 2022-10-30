using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;
using CIS.InternalServices.DocumentDataAggregator.Documents;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal static class DocumentMapperStatic
{
    public static void Test(DocumentConfiguration config, AggregatedData aggregatedData)
    {
        foreach (var sourceField in config.SourceFields)
        {
            if (sourceField.SourceFieldId.HasValue && config.DynamicStringFormats.Contains(sourceField.SourceFieldId.Value))
            {
                foreach (var dynamicStringFormat in config.DynamicStringFormats[sourceField.SourceFieldId.Value])
                {
                    foreach (var condition in dynamicStringFormat.Conditions)
                    {
                        if (condition.SourceFieldPath.Contains("[]"))
                        {
                            var collectionPaths = condition.SourceFieldPath.Split("[]");

                            var collection = aggregatedData.GetValue(collectionPaths[0]);


                        }
                        else
                        {
                            var value = aggregatedData.GetValue(condition.SourceFieldPath);
                        }
                    }
                }
            }
        }
    }

    public static IReadOnlyCollection<DocumentFieldData> Map(IReadOnlyCollection<SourceField> fields, AggregatedData aggregatedData)
    {
        return fields.Select(f => new
                     {
                         SourceField = f,
                         Value = aggregatedData.GetValue(f.FieldPath)
                     })
                     .Where(f => FilterEmptyValues(f.Value))
                     .Select(f => new DocumentFieldData
                     {
                         FieldName = f.SourceField.TemplateFieldName,
                         Value = f.Value!,
                         StringFormat = f.SourceField.StringFormat
                     })
                     .ToList();
    }

    private static bool FilterEmptyValues(object? obj) =>
        obj switch
        {
            null => false,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => true
        };
}