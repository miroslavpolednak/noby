using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Security;
using DomainServices.CustomerService.Api;

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

var x = builder.Configuration.GetSection("AppConfiguration");

builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);

appConfiguration.CheckAppConfiguration();

#endregion strongly typed configuration

#region register builder.Services
// strongly-typed konfigurace aplikace
builder.Services.AddSingleton(appConfiguration);

// globalni nastaveni prostredi
builder.Services.AddCisEnvironmentConfiguration(builder.Configuration);

// logging 
builder.Host.UseAppLogging();

// health checks
builder.Services.AddCisHealthChecks(builder.Configuration);

builder.Services.AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program));

// authentication
builder.Services.AddCisServiceAuthentication(builder.Configuration);

// add storage services
builder.Services.AddCutomerService(appConfiguration, builder.Configuration);

builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.CustomerService.Api.Services.CustomerService>();

    endpoints.MapGrpcReflectionService();
});

app.Run();
