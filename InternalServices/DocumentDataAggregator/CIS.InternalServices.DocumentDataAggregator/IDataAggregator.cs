using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.EasForms;

namespace CIS.InternalServices.DocumentDataAggregator;

public interface IDataAggregator
{
    Task<ICollection<DocumentFieldData>> GetDocumentData(Document document, string documentVersion, InputParameters input);

    Task<IEasForm<TData>> GetEasForm<TData>(int salesArrangementId) where TData : IEasFormData;
}