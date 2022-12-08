using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using DomainServices.CustomerService.Api.Extensions;
using CIS.InternalServices;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc"));

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

builder.Services.AddCisGrpcInfrastructure(typeof(Program));
builder.AddCustomerService();

builder.AddExternalServiceMpHome();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
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
app.UseCisLogging();

app.MapCisHealthChecks();

app.MapGrpcService<DomainServices.CustomerService.Api.Services.CustomerService>();

app.MapGrpcReflectionService();

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}
