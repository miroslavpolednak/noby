using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Core.Types;

string env = args[0];
string uri = args[1];
Console.WriteLine("service discovery " + uri);

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
    //builder.AddConfiguration(configuration.GetSection("Logging"));
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
services.AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
 {
     EnvironmentName = env,
     DefaultApplicationKey = "console"
 });
services.AddHttpContextAccessor();
services.AddCisServiceDiscovery(uri, true);
var provider = services.BuildServiceProvider();
            
var svc = provider.GetRequiredService<IDiscoveryServiceAbstraction>();

var result = await svc.GetService(new ApplicationEnvironmentName(env), new CIS.Core.Types.ServiceName("CIS:Storage"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
Console.WriteLine(result.ServiceUrl);
var result3 = await svc.GetServices(new ApplicationEnvironmentName(env));
Console.WriteLine(result3);

Console.ReadKey();
