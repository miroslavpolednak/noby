using CIS.Core;
using CIS.Core.Configuration;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Console_CustomerService;
using Console_DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>();
services.AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration());
services.AddDocumentGeneratorService();
services.AddCisServiceDiscovery("https://localhost:5005");
services.AddDataAggregatorService("https://localhost:5099");

var foundServices = services
                    .Where(t => t is { Lifetime: ServiceLifetime.Singleton, ImplementationInstance: IIsServiceDiscoverable })
                    .Select(t => t.ImplementationInstance as IIsServiceDiscoverable)
                    .Where(t => t is not null && t.UseServiceDiscovery)
                    .ToList();

foundServices.ForEach(instance =>
{
    // nastavit URL ze ServiceDiscovery
    instance!.ServiceUrl = new Uri("https://localhost");
});

var serviceProvider = services.BuildServiceProvider();

var dataAggregatorService = serviceProvider.GetRequiredService<IDataAggregatorServiceClient>();

//await GenerateDocument(dataAggregator, serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>());
await BuildForms(dataAggregatorService);

Console.ReadKey();

//static async Task GenerateDocument(IDataAggregatorServiceClient dataAggregator, IDocumentGeneratorServiceClient documentGeneratorService)
//{
//    var input = new InputParameters { OfferId = 1160, UserId = 3048 };
//    //var input = new InputParameters { SalesArrangementId = 97 };

//    var documentType = DocumentTemplateType.KALKULHU;

//    var data = await dataAggregator.GetDocumentData(documentType, "001A", input);

//    var request = new GenerateDocumentRequest
//    {
//        DocumentTypeId = (int)documentType,
//        DocumentTemplateVersion = "001A",
//        OutputType = OutputFileType.OpenForm,
//        Parts =
//        {
//            new GenerateDocumentPart
//            {
//                DocumentTypeId = (int)documentType,
//                DocumentTemplateVersion = "001A"
//            }

//        },
//        DocumentFooter = new DocumentFooter
//        {
//            CaseId = input.CaseId,
//            OfferId = input.OfferId,
//            //ArchiveId = 123456789
//        }
//    };

//    request.Parts.First().FillDocumentPart(data);

//    var result = await documentGeneratorService.GenerateDocument(request);

//    await using var fileStream = File.Open($"D:\\MPSS\\TestPdf\\Results\\{documentType}.pdf", FileMode.Create);

//    result.Data.WriteTo(fileStream);
//}

static async Task BuildForms(IDataAggregatorServiceClient dataAggregator)
{
    var request = new GetEasFormRequest
    {
        SalesArrangementId = 1248,
        EasFormRequestType = EasFormRequestType.Product
    };

    var response = await dataAggregator.GetEasForm(request);
}