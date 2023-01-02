using CIS.Core;
using CIS.Core.Configuration;
using CIS.Core.Exceptions;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices;
using CIS.InternalServices.DataAggregator;
using CIS.InternalServices.DataAggregator.Configuration;
using CIS.InternalServices.DataAggregator.Documents;
using CIS.InternalServices.DataAggregator.EasForms;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Console_CustomerService;
using Console_DocumentDataAggregator;
using DomainServices;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>();
services.AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration());
services.AddDataAggregator("Data Source=localhost;Initial Catalog=DataAggregator;Persist Security Info=True;User ID=SA;Password=Test123456;TrustServerCertificate=True");
//services.AddDocumentGeneratorService("https://localhost:5009");
services.AddDocumentGeneratorService();
services.AddCisServiceDiscovery("https://localhost:5005");

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

var dataAggregator = serviceProvider.GetRequiredService<IDataAggregator>();

//await GenerateDocument(dataAggregator, serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>());
await BuildForms(dataAggregator);

Console.ReadKey();

static async Task GenerateDocument(IDataAggregator dataAggregator, IDocumentGeneratorServiceClient documentGeneratorService)
{
    var input = new InputParameters { OfferId = 1160, UserId = 3048 };
    //var input = new InputParameters { SalesArrangementId = 97 };

    var documentType = DocumentTemplateType.KALKULHU;

    var data = await dataAggregator.GetDocumentData(documentType, "001A", input);

    var request = new GenerateDocumentRequest
    {
        TemplateTypeId = (int)documentType,
        TemplateVersion = "001A",
        OutputType = OutputFileType.OpenForm,
        Parts =
        {
            new GenerateDocumentPart
            {
                TemplateTypeId = (int)documentType,
                TemplateVersion = "001A"
            }

        },
        DocumentFooter = new DocumentFooter
        {
            CaseId = input.CaseId,
            OfferId = input.OfferId,
            //ArchiveId = 123456789
        }
    };

    request.Parts.First().FillDocumentPart(data);

    var result = await documentGeneratorService.GenerateDocument(request);

    await using var fileStream = File.Open($"D:\\MPSS\\TestPdf\\Results\\{documentType}.pdf", FileMode.Create);

    result.Data.WriteTo(fileStream);
}

static async Task BuildForms(IDataAggregator dataAggregator)
{
    var easForm = await dataAggregator.GetEasForm<IProductFormData>(97);
    return;
    try
    {
        var test = easForm.BuildForms(Enumerable.Range(1, 3).Select(c => new DynamicFormValues
        {
            DocumentId = $"ID{c}",
            FormId = $"Form{c}"
        }));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}