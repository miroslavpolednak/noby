using Microsoft.Extensions.DependencyInjection;
using DomainServices.RiskIntegrationService.Abstraction;
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
    .AddRiskIntegrationService()
    .BuildServiceProvider();

var service = serviceProvider.GetService<DomainServices.RiskIntegrationService.Abstraction.CreditWorthiness.V2.ICreditWorthinessServiceAbstraction>() ?? throw new Exception();

Console.WriteLine("RUN 1");
var result = await service.Calculate(new DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest
{
    RiskBusinessCaseId = "xxx"
});
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
