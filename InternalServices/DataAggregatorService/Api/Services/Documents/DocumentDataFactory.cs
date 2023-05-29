using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

[TransientService, SelfService]
internal class DocumentDataFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DocumentDataFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AggregatedData CreateData(DocumentType documentType) =>
        documentType switch
        {
            DocumentType.NABIDKA or DocumentType.KALKULHU => new OfferTemplateData(),
            DocumentType.ZADOCERP => new DrawingTemplateData(),
            DocumentType.ZADOSTHU => _serviceProvider.GetRequiredService<LoanApplication3601TemplateData>(),
            DocumentType.ZADOSTHD => _serviceProvider.GetRequiredService<LoanApplication3602TemplateData>(),
            DocumentType.ZAOZMPAR => new GeneralChangeTemplateData(),
            DocumentType.ZAOZMDLU => _serviceProvider.GetRequiredService<CustomerChangeTemplateData>(),
            DocumentType.ZAODHUBN => new HUBNTemplateData(),
            DocumentType.ZUSTAVSI or DocumentType.PRISTOUP or DocumentType.ZADOSTHD_SERVICE => _serviceProvider.GetRequiredService<CustomerChange3602TemplateData>(),
            _ => new AggregatedData()
        };

    public IDocumentVersionDataProvider CreateVersionData(DocumentType documentType)
    {
        return documentType switch
        {
            DocumentType.ZADOSTHU or DocumentType.ZADOSTHD or DocumentType.ZADOSTHD_SERVICE => _serviceProvider.GetRequiredService<LoanApplicationVersionDataProvider>(),
            DocumentType.ZAOZMDLU => _serviceProvider.GetRequiredService<CustomerChangeVersionDataProvider>(),
            DocumentType.ZUSTAVSI or DocumentType.PRISTOUP => _serviceProvider.GetRequiredService<CustomerChange3602VersionDataProvider>(),
            _ => _serviceProvider.GetRequiredService<IDocumentVersionDataProvider>()
        };
    }
}