using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.UserService.Api;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Security;
using CIS.InternalServices;
using DomainServices.UserService.Api.Endpoints;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc"));

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
    // add distributed cache
    .AddCisDistributedCache()
    // add self
    .AddUserService()
    .Services
        // add CIS services
        .AddCisServiceDiscovery()
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapCisGrpcHealthChecks();
app.MapGrpcReflectionService();
app.MapGrpcService<UserService>();

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