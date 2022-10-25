using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;
using CIS.InternalServices.DocumentDataAggregator.Documents;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal static class DocumentMapper
{
    public static IReadOnlyCollection<DocumentFieldData> Map(IReadOnlyCollection<SourceField> fields, AggregatedData aggregatedData)
    {
        return fields.Select(f => new DocumentFieldData
        {
            FieldName = f.TemplateFieldName,
            Value = aggregatedData.GetValue(f.FieldPath),
            StringFormat = f.StringFormat
        }).ToList();
    }
}