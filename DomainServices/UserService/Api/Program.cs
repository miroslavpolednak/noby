using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Security;
using DomainServices.UserService.Api;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Infrastructure.Telemetry;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc"));

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
#endregion strongly typed configuration

#region register builder.Services
// strongly-typed konfigurace aplikace
builder.Services.AddSingleton(appConfiguration);

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

// add services
builder.Services.AddCisServiceDiscovery(true);
builder.AddUserService(appConfiguration);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisLogging();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.UserService.Api.Services.UserService>();

    endpoints.MapGrpcReflectionService();
});

app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}