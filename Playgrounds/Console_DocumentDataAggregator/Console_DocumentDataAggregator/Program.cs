using CIS.InternalServices.DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDataAggregator();

var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetRequiredService<IDataAggregator>().GetDocumentData(123);

Console.ReadKey();