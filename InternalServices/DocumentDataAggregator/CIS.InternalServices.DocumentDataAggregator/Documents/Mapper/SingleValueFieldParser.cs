using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Mapper;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal class SingleValueFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, SourceField> sourceFields, AggregatedData aggregatedData) =>
        sourceFields.Select(f => new DocumentMapper.SourceFieldData
        {
            SourceFieldId = f.SourceFieldId,
            TemplateFieldName = f.TemplateFieldName,
            StringFormat = f.StringFormat,
            Value = MapperHelper.GetValue(aggregatedData, f.FieldPath)
        });
}