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
// globalni nastaveni prostredi
builder.AddCisEnvironmentConfiguration();

// logging 
builder.AddCisLogging();
builder.AddCisTracing();

// health checks
builder.AddCisHealthChecks();

builder.AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program));

// authentication
builder.AddCisServiceAuthentication();

builder.Services.AddCisServiceDiscovery(); // kvuli auto dotazeni URL pro EAS

builder.Services.AddCodebookService();

// add my services
builder.Services.AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init());
builder.AddOfferService();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});

// add BE services
builder.Services
    .AddGrpcReflection();
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

app.MapCisHealthChecks();

app.MapGrpcService<OfferService>();

app.MapGrpcReflectionService();

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