using CIS.InternalServices.DocumentDataAggregator.DataServices;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal interface ISourceFieldParser
{
    IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, SourceField> sourceFields, AggregatedData aggregatedData);
}