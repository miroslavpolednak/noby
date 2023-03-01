using CIS.Infrastructure.StartupExtensions;
using DomainServices.HouseholdService.Api;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using DomainServices;
using CIS.InternalServices;
using CIS.Infrastructure.gRPC;

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
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisHealthChecks();

// logging 
builder
    .AddCisLogging()
    .AddCisTracing();

// authentication
builder.AddCisServiceAuthentication();

// add services
builder.Services
    .AddCisServiceDiscovery()
    .AddCaseService()
    .AddCodebookService()
    .AddOfferService()
    .AddSalesArrangementService()
    .AddCustomerService()
    .AddUserService();

builder.Services.AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Messages);
builder.AddHouseholdService();

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

app.UseServiceDiscovery();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();
app.UseCisLogging();

app.MapCisHealthChecks();

app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.HouseholdService>();
app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.CustomerOnSAService>();

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