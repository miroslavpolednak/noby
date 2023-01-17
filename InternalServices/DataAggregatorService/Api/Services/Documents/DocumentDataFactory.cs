using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

internal static class DocumentDataFactory
{
    public static AggregatedData Create(DocumentType documentType) =>
        documentType switch
        {
            DocumentType.NABIDKA or DocumentType.KALKULHU => new OfferTemplateData(),
            DocumentType.ZADOCERP => new LoanApplicationTemplateData(),
            _ => new AggregatedData()
        };
}