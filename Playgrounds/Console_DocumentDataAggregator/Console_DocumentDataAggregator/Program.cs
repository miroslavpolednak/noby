using CIS.InternalServices.DocumentDataAggregator;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentGeneratorService.Api.Services;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDataAggregator();
services.AddDocumentGeneratorService("https://localhost:5009");

var serviceProvider = services.BuildServiceProvider();

var input = new InputParameters { OfferId = 111 };

var data = await serviceProvider.GetRequiredService<IDataAggregator>().GetDocumentData(input);

//var request = new GenerateDocumentRequest
//{
//    Parts = { new GenerateDocumentPart() }
//};

//foreach (var documentData in data)
//{
//    request.Parts.First().Data.Add(new GenerateDocumentPartData
//    {
//        Key = documentData.FieldName,
//        StringFormat = documentData.StringFormat ?? string.Empty
//    }.SetDocumentPartDataValue(documentData.Value));
//}

//new PdfDocumentManager().GenerateDocument(request);

//await serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>().GenerateDocument(request);

Console.ReadKey();