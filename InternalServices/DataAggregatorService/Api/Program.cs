using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using CIS.InternalServices.DataAggregatorService.Api;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Endpoints;
using DomainServices;
using Microsoft.EntityFrameworkCore;

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

    builder.Services.AddDbContext<ConfigurationContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("default")));

    builder.Services
           .AddAttributedServices(typeof(Program))
           .AddCisGrpcInfrastructure(typeof(Program))
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