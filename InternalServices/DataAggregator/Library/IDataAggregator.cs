using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Documents;
using CIS.InternalServices.DataAggregator.EasForms;

namespace CIS.InternalServices.DataAggregator;

public interface IDataAggregator
{
    Task<ICollection<DocumentFieldData>> GetDocumentData(DocumentTemplateType document, string documentVersion, InputParameters input);

    Task<IEasForm<TData>> GetEasForm<TData>(int salesArrangementId) where TData : IEasFormData;
}