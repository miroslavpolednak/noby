using Microsoft.Extensions.DependencyInjection;
using DomainServices.CodebookService.Abstraction;

Console.WriteLine("run!");

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = "https://localhost:5005",
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddHttpContextAccessor()
    .AddCodebookService(true)
    .BuildServiceProvider();

var service = serviceProvider.GetService<DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction>() ?? throw new Exception();

Console.WriteLine("RUN 1");
var result = await service.ProductTypes();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

Console.WriteLine("RUN 2");
var result2 = await service.MyTestCodebook();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result2));
