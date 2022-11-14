using CIS.InternalServices.DocumentDataAggregator.Documents;

namespace CIS.InternalServices.DocumentDataAggregator;

public interface IDataAggregator
{
    Task<IReadOnlyCollection<DocumentFieldData>> GetDocumentData(Document document, string documentVersion, InputParameters input);
}