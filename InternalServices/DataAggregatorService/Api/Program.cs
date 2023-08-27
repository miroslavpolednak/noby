using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using CIS.InternalServices.DataAggregatorService.Api;
using CIS.InternalServices.DataAggregatorService.Api.Configuration;
using CIS.InternalServices.DataAggregatorService.Api.Endpoints;
using DomainServices;

var runAsWinSvc = args.Any(t => t.Equals("winsvc"));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = runAsWinSvc ? AppContext.BaseDirectory : default
});

var log = builder.CreateStartupLogger();

try
{
    var config = new DataAggregatorConfiguration();
    builder.Configuration.Bind("DataAggregatorConfiguration", config);

    builder.Services.AddSingleton(config);

    builder.AddCisEnvironmentConfiguration();
    builder.AddCisCoreFeatures()
           .AddCisLogging()
           .AddCisTracing()
           .AddCisServiceAuthentication();

    builder.Services
           .AddCisServiceDiscovery()
           .AddCodebookService()
           .AddSalesArrangementService()
           .AddCaseService()
           .AddOfferService()
           .AddUserService()
           .AddCustomerService()
           .AddProductService()
           .AddHouseholdService()
           .AddDocumentOnSAService();

    builder.Services.AddDapper(builder.Configuration.GetConnectionString("default")!);

    builder.Services
           .AddAttributedServices(typeof(CIS.InternalServices.DataAggregatorService.Api.Program))
           .AddCisGrpcInfrastructure(typeof(CIS.InternalServices.DataAggregatorService.Api.Program))
           .AddGrpcReflection()
           .AddGrpc(opts => opts.Interceptors.Add<GenericServerExceptionInterceptor>());

    builder.AddCisGrpcHealthChecks();

    if (config.UseCacheForConfiguration)
    {
        builder.Services.AddMemoryCache();
        builder.Services.Decorate<IConfigurationManager, CachedConfigurationManager>();
    }

    if (runAsWinSvc) builder.Host.UseWindowsService();

    var app = builder.UseKestrelWithCustomConfiguration().Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCisServiceUserContext();

    app.MapCisGrpcHealthChecks();

    app.MapGrpcService<DataAggregatorServiceGrpc>();
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

namespace CIS.InternalServices.DataAggregatorService.Api
{
    public partial class Program
    {
        // Expose the Program class for use with WebApplicationFactory<T>
    }
}
