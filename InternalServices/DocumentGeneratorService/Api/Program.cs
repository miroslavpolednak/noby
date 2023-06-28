using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using CIS.InternalServices.DocumentGeneratorService.Api;
using CIS.InternalServices.DocumentGeneratorService.Api.Services;
using DomainServices;
using Serilog;

var runAsWinSvc = args.Any(t => t.Equals("winsvc"));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = runAsWinSvc ? AppContext.BaseDirectory : default
});

var log = builder.CreateStartupLogger();

try
{
    builder.AddCisEnvironmentConfiguration(); // globalni nastaveni prostredi
    builder
           .AddCisCoreFeatures()
           .AddCisLogging()
           .AddCisTracing()
           .AddCisServiceAuthentication();

    builder.Services.Configure<GeneratorConfiguration>(builder.Configuration.GetRequiredSection("GeneratorConfiguration"));

    builder.Services
           .AddCisServiceDiscovery()
           .AddCodebookService();

    builder.Services.AddAttributedServices(typeof(Program));

    builder.Services
        .AddCisGrpcInfrastructure(typeof(Program))
        .AddGrpcReflection()
        .AddGrpc(options =>
        {
            options.Interceptors.Add<GenericServerExceptionInterceptor>();
        });
    builder.AddCisGrpcHealthChecks();

    if (runAsWinSvc) builder.Host.UseWindowsService();

    var app = builder.UseKestrelWithCustomConfiguration().Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapCisGrpcHealthChecks();

    app.MapGrpcService<DocumentGeneratorService>();
    app.MapGrpcReflectionService();

    log.ApplicationRun();
    app.Run();
}
catch (Exception ex)
{
    log.CatchedException(ex);
}
finally
{
    LoggingExtensions.CloseAndFlush();
}