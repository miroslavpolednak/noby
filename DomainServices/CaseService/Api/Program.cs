using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api;
using DomainServices;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using CIS.InternalServices;
using DomainServices.CaseService.Api.Endpoints;
using CIS.Infrastructure.CisMediatR;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

#region register builder
builder.Services.AddAttributedServices(typeof(Program));

// globalni nastaveni prostredi
builder
    .AddCisCoreFeatures()
    .AddCisEnvironmentConfiguration();

// logging 
builder
    .AddCisLogging()
    .AddCisTracing();

// authentication
builder.AddCisServiceAuthentication();

// add BE services
builder.Services
    .AddSalesArrangementService()
    .AddCodebookService()
    .AddUserService()
    .AddCisServiceDiscovery();

builder.Services.AddCisMediatrRollbackCapability();

// add this service
builder.AddCaseService();

builder.Services
    .AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init())
    .AddGrpcReflection()
    .AddGrpc(options =>
    {
        options.Interceptors.Add<GenericServerExceptionInterceptor>();
    });
builder.AddCisGrpcHealthChecks();
#endregion register builder

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
app.MapGrpcService<CaseService>();

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