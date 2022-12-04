using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CIS.InternalServices.ServiceDiscovery.Clients;
using CIS.Core.Types;
using System.Linq;

string env = args.Any() ? args[0] : "DEV";
string uri = args.Any() && args.Length > 1 ? args[1] : "https://127.0.0.1:5005";
Console.WriteLine($"service discovery for {env} on {uri}");

//setup our DI
var services = new ServiceCollection();

var builder = new ConfigurationBuilder()
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
var configuration = builder.Build();
services.AddSingleton(configuration);
services.AddSingleton<ILoggerFactory, LoggerFactory>();
services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Debug);
});
services.AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
 {
    ServiceDiscoveryUrl = uri,
    InternalServicePassword = "a",
    InternalServicesLogin = "a",
    EnvironmentName = env,
    DefaultApplicationKey = "console"
 });
services.AddHttpContextAccessor();
services.AddCisServiceDiscovery();
var provider = services.BuildServiceProvider();
            
var svc = provider.GetRequiredService<IDiscoveryServiceAbstraction>();

var result = await svc.GetService(new(env), new("DS:OfferService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
Console.WriteLine(result.ServiceUrl);
var result3 = await svc.GetServices(new ApplicationEnvironmentName(env));
Console.WriteLine(result3);

Console.ReadKey();
