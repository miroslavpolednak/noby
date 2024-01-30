using SharedAudit;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Api.Endpoints;
using DomainServices.DocumentOnSAService.Api.Extensions;

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
    #region register builder
    // globalni nastaveni prostredi
    builder
        .AddCisCoreFeatures()
        .AddCisEnvironmentConfiguration();

    // logging 
    builder
        .AddCisLogging()
        .AddCisLoggingPayloadBehavior()
        .AddCisTracing()
        .AddCisAudit();

    // authentication
    builder.AddCisServiceAuthentication().AddCisServiceUserContext();

    // add this service
    builder.AddDocumentOnSAServiceService();

    builder.AddDocumentOnSAServiceGrpc();
    builder.AddCisGrpcHealthChecks();
    #endregion

    // kestrel configuration
    builder.UseKestrelWithCustomConfiguration();

    // BUILD APP
    if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCisServiceUserContext();

    //Dont know correct connection
    app.UseServiceDiscovery();

    app.MapCisGrpcHealthChecks();
    app.MapGrpcService<DocumentOnSAServiceGrpc>();
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

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
