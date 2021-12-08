using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.ServiceDiscovery.Api;

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
builder.Services.AddCisEnvironmentConfiguration(builder.Configuration);

// logging 
//builder.Host.UseAppLogging();

// add mediatr
builder.Services.AddMediatR(typeof(Program).Assembly);

// health checks
builder.Services.AddCisHealthChecks(builder.Configuration);
builder.Host.AddCisLogging();
builder.Services.AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program));

// add general Dapper repository
builder.Services.AddDapper(builder.Configuration.GetConnectionString("default"));

// current project related
// add cache
builder.Services.AddServiceDiscoveryCache(appConfiguration);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.SimpleServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<CIS.InternalServices.ServiceDiscovery.Api.Services.DiscoveryService>();

    endpoints.MapGrpcReflectionService();
});

app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}