﻿using Microsoft.Extensions.DependencyInjection;
using DomainServices.CodebookService.Clients;
using CIS.DomainServicesSecurity.ContextUser;

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
    .AddScoped<CIS.Core.Security.ICurrentUserAccessor, CisCurrentContextUserAccessor>()
    .AddHttpContextAccessor()
    .AddCodebookService()
    .BuildServiceProvider();

var service = serviceProvider.GetService<DomainServices.CodebookService.Clients.ICodebookServiceClients>() ?? throw new Exception();

Console.WriteLine("RUN 1");
var result = await service.ProductTypes();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

Console.WriteLine("RUN 2");
var result2 = await service.MyTestCodebook();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result2));
