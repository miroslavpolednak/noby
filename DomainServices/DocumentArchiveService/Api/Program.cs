using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using DomainServices.DocumentArchiveService.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

builder.Services.AddOptions<AppConfiguration>()
    .Bind(builder.Configuration.GetSection(AppConfiguration.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

#region register builder

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

// add this service
builder.AddDocumentArchiveService(builder.Configuration);

// add grpc
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
builder.AddDocumentArchiveGrpc();

// add grpc swagger 
builder.AddDocumentArchiveGrpcSwagger();

#endregion register builder

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

app.UseDocumentArchiveGrpcSwagger();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();

app.UseCisLogging();

app.UseServiceDiscovery();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.DocumentArchiveService.Api.Endpoints.DocumentArchiveServiceGrpc>();

    endpoints.MapGrpcReflectionService();
});

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