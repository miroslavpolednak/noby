using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Api;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using CIS.InternalServices;
using DomainServices;
using DomainServices.OfferService.Api.Endpoints;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

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
    .AddOfferService()
    .Services
        // add CIS services
        .AddCisServiceDiscovery()
        .AddCodebookService()
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

app.UseServiceDiscovery();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();

app.MapCisGrpcHealthChecks();
app.MapGrpcReflectionService();
app.MapGrpcService<OfferService>();

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}