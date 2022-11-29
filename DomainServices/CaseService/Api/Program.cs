using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api;
using DomainServices.CodebookService.Clients;
using DomainServices.UserService.Clients;
using CIS.InternalServices.ServiceDiscovery.Clients;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using DomainServices.SalesArrangementService.Clients;

string s = "";

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

#region strongly typed configuration
AppConfiguration appConfiguration = new();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
appConfiguration.CheckAppConfiguration();
#endregion strongly typed configuration

#region register builder
// strongly-typed konfigurace aplikace
builder.Services.AddSingleton(appConfiguration);

// globalni nastaveni prostredi
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures();

// logging 
builder
    .AddCisLogging()
    .AddCisTracing();

// health checks
builder.AddCisHealthChecks();
builder.Services.AddAttributedServices(typeof(Program));

// authentication
builder.AddCisServiceAuthentication();

// add BE services
builder.Services
    .AddSalesArrangementService()
    .AddCodebookService()
    .AddUserService()
    .AddCisServiceDiscovery();

builder.Services.AddCisGrpcInfrastructure(typeof(Program));
// add this service
builder.AddCaseService(appConfiguration);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
#endregion register builder

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();
app.UseCisLogging();

app.MapCisHealthChecks();

app.MapGrpcService<DomainServices.CaseService.Api.Services.CaseService>();

app.MapGrpcReflectionService();

try
{
    app.Run();
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