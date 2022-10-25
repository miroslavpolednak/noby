using CIS.InternalServices.DocumentDataAggregator;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using Console_DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDataAggregator();

var serviceProvider = services.BuildServiceProvider();

var input = new InputParameters { OfferId = 111 };

var data = await serviceProvider.GetRequiredService<IDataAggregator>().GetDocumentData(input);

var dataToFormat = data.First(x => x.FieldName == "LoanAmount");

var s = string.Format(new CustomFormatter(), dataToFormat.StringFormat!, (decimal)dataToFormat.Value);

 Console.ReadKey();