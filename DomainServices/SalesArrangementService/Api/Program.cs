using CIS.Infrastructure.StartupExtensions;
using DomainServices.SalesArrangementService.Api;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using DomainServices;
using CIS.InternalServices;
using DomainServices.SalesArrangementService.Api.Endpoints;
using Serilog;

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
    builder.Services.AddAttributedServices(typeof(Program));

    // globalni nastaveni prostredi
    builder
        .AddCisCoreFeatures()
        .AddCisEnvironmentConfiguration();

    builder
        // logging 
        .AddCisLogging()
        .AddCisTracing()
        // authentication
        .AddCisServiceAuthentication()
        // add self
        .AddSalesArrangementService()
        // add services
        .Services
            // add CIS services
            .AddCisServiceDiscovery()
            .AddCaseService()
            .AddCodebookService()
            .AddOfferService()
            .AddUserService()
            .AddHouseholdService()
            .AddDocumentArchiveService()
            .AddDocumentOnSAService()
            .AddDocumentGeneratorService()
            .AddDataAggregatorService()
            // add grpc infrastructure
            .AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init())
            .AddGrpcReflection()
            .AddGrpc(options =>
            {
                options.Interceptors.Add<GenericServerExceptionInterceptor>();
            });

    // add HC
    builder.AddCisGrpcHealthChecks();
    #endregion register builder.Services

    // kestrel configuration
    builder.UseKestrelWithCustomConfiguration();

    // BUILD APP
    if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCisServiceUserContext();

    app.MapCisGrpcHealthChecks();
    app.MapGrpcReflectionService();
    app.MapGrpcService<SalesArrangementService>();

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