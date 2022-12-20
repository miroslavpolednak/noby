using CIS.InternalServices.DataAggregator.Configuration.Document;
using CIS.InternalServices.DataAggregator.DataServices;

namespace CIS.InternalServices.DataAggregator.Documents.Mapper;

internal interface ISourceFieldParser
{
    IEnumerable<DocumentMapper.SourceFieldData> GetFields(IGrouping<string, DocumentSourceField> sourceFields, AggregatedData aggregatedData);
}