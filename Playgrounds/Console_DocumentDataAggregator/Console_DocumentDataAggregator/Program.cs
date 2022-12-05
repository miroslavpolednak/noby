using CIS.Core.Configuration;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices.DocumentDataAggregator;
using CIS.InternalServices.DocumentDataAggregator.Configuration;
using CIS.InternalServices.DocumentDataAggregator.EasForms;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Console_CustomerService;
using Console_DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>();
services.AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration());
services.AddDataAggregator();
services.AddDocumentGeneratorService("https://localhost:5009");
services.AddCisServiceDiscovery("https://localhost:5005");

var serviceProvider = services.BuildServiceProvider();
var dataAggregator = serviceProvider.GetRequiredService<IDataAggregator>();

await GenerateDocument(dataAggregator, serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>());

Console.ReadKey();

static async Task GenerateDocument(IDataAggregator dataAggregator, IDocumentGeneratorServiceClient documentGeneratorService)
{
    //var input = new InputParameters { OfferId = 554, UserId = 3048 };
    var input = new InputParameters { SalesArrangementId = 97 };

    var documentType = DocumentTemplateType.NABIDKA;

    var data = await dataAggregator.GetDocumentData(documentType, "001A", input);

    var request = new GenerateDocumentRequest
    {
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
            TemplateTypeId = (int)documentType,
            TemplateVersion = "001A",
            CaseId = input.CaseId,
            OfferId = input.OfferId,
            //ArchiveId = 123456789
        }
    };

    foreach (var documentData in data)
    {
        request.Parts
               .First()
               .Data
               .Add(new GenerateDocumentPartData
               {
                   Key = documentData.FieldName,
                   StringFormat = documentData.StringFormat
               }.SetDocumentPartDataValue(documentData.Value));
    }

    var result = await documentGeneratorService.GenerateDocument(request);

    await using var fileStream = File.Open($"D:\\MPSS\\TestPdf\\Results\\{documentType}.pdf", FileMode.Create);

    result.Data.WriteTo(fileStream);
}

static async Task BuildForms(IDataAggregator dataAggregator)
{
    var easForm = await dataAggregator.GetEasForm<IProductFormData>(97);

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