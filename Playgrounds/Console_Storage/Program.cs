using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.Storage.Abstraction;
using System;

Console.WriteLine("Hello World!");

//setup our DI
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
    .AddStorage(true)
    .AddHttpContextAccessor()
    .BuildServiceProvider();

var service = serviceProvider.GetService<CIS.InternalServices.Storage.Abstraction.IBlobServiceAbstraction>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
var result = await service.Save(System.Text.UTF8Encoding.UTF8.GetBytes("aaaaaa"));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
Console.WriteLine(result);

var result2 = await service.Get(result);
Console.WriteLine(result2.Name);

await service.Delete(result);
Console.WriteLine("deleted");

Console.ReadKey();
