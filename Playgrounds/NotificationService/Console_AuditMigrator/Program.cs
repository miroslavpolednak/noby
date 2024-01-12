// See https://aka.ms/new-console-template for more information

using CIS.Core.Types;
using Console_AuditMigrator;
using Console_AuditMigrator.Database;
using Console_AuditMigrator.Services;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Starting.");

// setup
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddCommandLine(args)
    .Build();

var services = new ServiceCollection();

services
    .AddSingleton<IConfiguration>(configuration)
    .AddOptions<AppConfiguration>()
    .Bind(configuration.GetSection(nameof(AppConfiguration)));

var serviceProvider = services
    .AddServices(configuration)
    .BuildServiceProvider();


// services
var application = serviceProvider.GetRequiredService<IApplication>();
await application.Run();

var notificationServiceContext = serviceProvider.GetRequiredService<NotificationServiceContext>();

Console.WriteLine("Done.");