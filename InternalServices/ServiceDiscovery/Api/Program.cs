using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.ServiceDiscovery.Api;
using CIS.InternalServices.ServiceDiscovery.Api.Endpoints;
using Microsoft.AspNetCore.Mvc;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

var log = builder.CreateStartupLogger();

try
{
    #region register builder.Services
    // globalni nastaveni prostredi
    var envConfiguration = builder
        .AddCisCoreFeatures()
        .AddCisEnvironmentConfiguration();
    builder.Services.AddAttributedServices(typeof(Program));

    // add mediatr
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // logging, tracing
    builder
        .AddCisLogging()
        .AddCisTracing();

    // add general Dapper repository
    builder.AddBaseEntityFramework<CIS.InternalServices.ServiceDiscovery.Api.Database.ServiceDiscoveryDbContext>();

    // add GRPC
    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<GenericServerExceptionInterceptor>();
    }).AddJsonTranscoding();
    builder.Services
        .AddGrpcReflection()
        .AddServiceDiscoverySwagger();
    #endregion register builder.Services

    // kestrel configuration
    builder.UseKestrelWithCustomConfiguration();

    // BUILD APP
    if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseHsts();
    app.UseRouting();

    app.MapGrpcReflectionService();
    app.UseServiceDiscoverySwagger();

    app.MapGrpcService<DiscoveryService>();
    // add global healthcheck as json api
    app.MapGet(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, async (
                [FromServices] CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GlobalHealtCheck.GlobalHealthCheckHandler handler,
                CancellationToken cancellationToken) 
                => await handler.Handle(cancellationToken))
        .WithTags("Monitoring")
        .WithName("Globální HealthCheck endpoint sdružující HC všech služeb v NOBY.")
        .WithOpenApi();

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

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}