using CIS.Core.Configuration;
using CIS.Core.Security;
using CIS.InternalServices.DocumentDataAggregator;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using CIS.InternalServices.DocumentGeneratorService.Api.Services;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Console_CustomerService;
using Console_DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;
using Document = CIS.InternalServices.DocumentDataAggregator.Documents.Document;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>();
services.AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration());
services.AddDataAggregator();
services.AddDocumentGeneratorService("https://localhost:5009");
services.AddCisServiceDiscovery("https://localhost:5005");

var serviceProvider = services.BuildServiceProvider();

//var input = new InputParameters { OfferId = 554, UserId = 3048 };
var input = new InputParameters { SalesArrangementId = 180 };

var data = await serviceProvider.GetRequiredService<IDataAggregator>().GetDocumentData(Document.Offer, "001A", input);

var request = new GenerateDocumentRequest
{
    Parts = { new GenerateDocumentPart() }
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

new PdfDocumentManager().GenerateDocument(request);

//await serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>().GenerateDocument(request);

Console.ReadKey();