using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using DomainServices.DocumentArchiveService.Api;

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
    #region strongly typed configuration
    AppConfiguration appConfiguration = new();
    builder.Configuration.GetSection(AppConfiguration.SectionName).Bind(appConfiguration);
    appConfiguration.CheckAppConfiguration();
    #endregion strongly typed configuration

    #region register builder
    builder.Services.AddAttributedServices(typeof(Program));

    // strongly-typed konfigurace aplikace
    builder.Services.AddSingleton(appConfiguration);

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
        .AddDocumentArchiveService()
        .Services
            // add CIS services
            .AddCisServiceDiscovery()
            // add grpc infrastructure
            .AddCisGrpcInfrastructure(typeof(Program));

    // add grpc
    builder.AddDocumentArchiveGrpc();

    // add grpc swagger 
    builder.AddDocumentArchiveGrpcSwagger();
    builder.AddCisGrpcHealthChecks();
    #endregion register builder

    // kestrel configuration
    builder.UseKestrelWithCustomConfiguration();

    // BUILD APP
    if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseRouting();

    app.UseDocumentArchiveGrpcSwagger();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCisServiceUserContext();

    //Dont know correct connection
    app.UseServiceDiscovery();

    app.MapCisGrpcHealthChecks();
    app.MapGrpcService<DomainServices.DocumentArchiveService.Api.Endpoints.DocumentArchiveServiceGrpc>();
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