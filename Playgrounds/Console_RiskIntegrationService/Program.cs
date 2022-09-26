﻿using Microsoft.Extensions.DependencyInjection;
using DomainServices.RiskIntegrationService.Clients;
using CIS.DomainServicesSecurity.ContextUser;
using CIS.Core.Results;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using Console_RiskIntegrationService;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

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
    //.AddRiskIntegrationService("https://127.0.0.1:5022")
    .AddRiskIntegrationService("https://172.30.35.51:30012")
    .BuildServiceProvider();

/*Console.WriteLine("RUN 1");
var service1 = serviceProvider.GetService<DomainServices.RiskIntegrationService.Abstraction.CreditWorthiness.V2.ICreditWorthinessServiceAbstraction>() ?? throw new Exception();
var result1 = ServiceCallResult.ResolveAndThrowIfError<CreditWorthinessCalculateResponse>(await service1.Calculate(CreditWorthinessTest._test1));
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result1));*/

Console.WriteLine("RUN 2");
var service2 = serviceProvider.GetService<DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceAbstraction>() ?? throw new Exception();
var result2 = ServiceCallResult.ResolveAndThrowIfError<LoanApplicationSaveResponse>(await service2.Save(LoanApplicationTest._test1));
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result2));

/*Console.WriteLine("RUN 3");
var service3 = serviceProvider.GetService<DomainServices.RiskIntegrationService.Abstraction.CustomersExposure.V2.ICustomersExposureServiceAbstraction>() ?? throw new Exception();
var result3 = ServiceCallResult.ResolveAndThrowIfError<CustomersExposureCalculateResponse>(await service3.Calculate(ExposureTest._test1));
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result3));*/