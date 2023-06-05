using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using DomainServices.CustomerService.Api.Extensions;
using CIS.InternalServices;
using ExternalServices;
using DomainServices.CustomerService.Api.Endpoints;

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
    .AddCustomerService()
    // add external svc
    .AddExternalService<ExternalServices.MpHome.V1.IMpHomeClient>()
    .Services
    // add grpc infrastructure
        .AddCisGrpcInfrastructure(typeof(Program))
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
app.MapGrpcService<CustomerService>();

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}
