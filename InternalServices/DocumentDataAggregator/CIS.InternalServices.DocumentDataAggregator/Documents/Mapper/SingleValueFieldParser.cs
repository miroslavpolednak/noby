using CIS.InternalServices.DocumentDataAggregator.Configuration.Document;
using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal class SingleValueFieldParser : ISourceFieldParser
{
    public IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, DocumentSourceField> sourceFields, AggregatedData aggregatedData) =>
        sourceFields.Select(f => new DocumentMapper.SourceFieldData
        {
            SourceFieldId = f.SourceFieldId,
            AcroFieldName = f.AcroFieldName,
            StringFormat = f.StringFormat,
            Value = MapperHelper.GetValue(aggregatedData, f.FieldPath)
        });
}