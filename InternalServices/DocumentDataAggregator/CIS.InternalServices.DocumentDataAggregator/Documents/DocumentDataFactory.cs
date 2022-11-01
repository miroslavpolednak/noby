using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents.TemplateData;

namespace CIS.InternalServices.DocumentDataAggregator.Documents;

internal static class DocumentDataFactory
{
    public static AggregatedData Create(Document document) =>
        document switch
        {
            Document.Offer => new OfferTemplateData(),
            _ => new AggregatedData()
        };
}