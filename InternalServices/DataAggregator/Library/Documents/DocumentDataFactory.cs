using CIS.Foms.Enums;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

namespace CIS.InternalServices.DocumentDataAggregator.Documents;

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