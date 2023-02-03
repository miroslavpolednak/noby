using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Api.Configuration;
using DomainServices.DocumentOnSAService.Api.Endpoints;
using DomainServices.DocumentOnSAService.Api.Extensions;

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
builder.Configuration.GetSection(AppConfiguration.SectionName).Bind(appConfiguration);
appConfiguration.CheckAppConfiguration();
#endregion strongly typed configuration

#region register builder
// strongly-typed app configuration
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

// authentication
builder.AddCisServiceAuthentication();

// add this service
builder.AddDocumentOnSAServiceService();

builder.AddDocumentOnSAServiceGrpc();

#endregion

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

//Dont know correct connection
app.UseServiceDiscovery();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DocumentOnSAServiceGrpc>();

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