using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

[TransientService, SelfService]
internal class DocumentDataFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DocumentDataFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AggregatedData Create(DocumentType documentType) =>
        documentType switch
        {
            DocumentType.NABIDKA or DocumentType.KALKULHU => new OfferTemplateData(),
            DocumentType.ZADOCERP => new DrawingTemplateData(),
            DocumentType.ZADOSTHU => _serviceProvider.GetRequiredService<LoanApplication3601TemplateData>(),
            DocumentType.ZADOSTHD => _serviceProvider.GetRequiredService<LoanApplication3602TemplateData>(),
            DocumentType.ZAOZMPAR => new GeneralChangeTemplateData(),
            DocumentType.ZAODHUBN => new HUBNTemplateData(),
            DocumentType.ZAOZMDLU => _serviceProvider.GetRequiredService<CustomerChangeTemplateData>(),
            _ => new AggregatedData()
        };
}