using CIS.Foms.Enums;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.EasForms;

namespace CIS.InternalServices.DocumentDataAggregator;

public interface IDataAggregator
{
    Task<ICollection<DocumentFieldData>> GetDocumentData(DocumentTemplateType document, string documentVersion, InputParameters input);

    Task<IEasForm<TData>> GetEasForm<TData>(int salesArrangementId) where TData : IEasFormData;
}