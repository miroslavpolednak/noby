using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.DataServices;
using CIS.InternalServices.DataAggregator.Documents.TemplateData;

namespace CIS.InternalServices.DataAggregator.Documents;

internal static class DocumentDataFactory
{
    public static AggregatedData Create(DocumentTemplateType documentType) =>
        documentType switch
        {
            DocumentTemplateType.NABIDKA or DocumentTemplateType.KALKULHU => new OfferTemplateData(),
            DocumentTemplateType.ZADOCERP => new LoanApplicationTemplateData(),
            _ => new AggregatedData()
        };
}