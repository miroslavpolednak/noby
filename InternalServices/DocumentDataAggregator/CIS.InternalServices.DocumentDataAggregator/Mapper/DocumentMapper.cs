using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal static class DocumentMapper
{
    public static Dictionary<string, object?> Map(IEnumerable<DataSourceField> fields, AggregatedData aggregatedData)
    {
        return fields.ToDictionary(f => f.TemplateFieldName, f => aggregatedData.GetValue(f.Path));
    }
}