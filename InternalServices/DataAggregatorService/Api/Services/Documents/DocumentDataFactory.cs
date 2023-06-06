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

    public AggregatedData CreateData(DocumentTypes documentType) =>
        documentType switch
        {
            DocumentTypes.NABIDKA or DocumentTypes.KALKULHU => new OfferTemplateData(),
            DocumentTypes.ZADOCERP => new DrawingTemplateData(),
            DocumentTypes.ZADOSTHU => _serviceProvider.GetRequiredService<LoanApplication3601TemplateData>(),
            DocumentTypes.ZADOSTHD => _serviceProvider.GetRequiredService<LoanApplication3602TemplateData>(),
            DocumentTypes.ZAOZMPAR => new GeneralChangeTemplateData(),
            DocumentTypes.ZAOZMDLU => _serviceProvider.GetRequiredService<CustomerChangeTemplateData>(),
            DocumentTypes.ZAODHUBN => new HUBNTemplateData(),
            DocumentTypes.ZUSTAVSI or DocumentTypes.PRISTOUP or DocumentTypes.ZADOSTHD_SERVICE => _serviceProvider.GetRequiredService<CustomerChange3602TemplateData>(),
            _ => new AggregatedData()
        };

    public IDocumentVersionDataProvider CreateVersionData(DocumentTypes documentType)
    {
        return documentType switch
        {
            DocumentTypes.ZADOSTHU or DocumentTypes.ZADOSTHD or DocumentTypes.ZADOSTHD_SERVICE => _serviceProvider.GetRequiredService<LoanApplicationVersionDataProvider>(),
            DocumentTypes.ZAOZMDLU => _serviceProvider.GetRequiredService<CustomerChangeVersionDataProvider>(),
            DocumentTypes.ZUSTAVSI or DocumentTypes.PRISTOUP => _serviceProvider.GetRequiredService<CustomerChange3602VersionDataProvider>(),
            _ => _serviceProvider.GetRequiredService<IDocumentVersionDataProvider>()
        };
    }
}