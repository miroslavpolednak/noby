using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CodebookService.Abstraction;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Infrastructure.Telemetry;
using DomainServices.RiskIntegrationService.Api;
using CIS.DomainServicesSecurity;
using Microsoft.OpenApi.Models;

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
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
#endregion strongly typed configuration

#region register builder
// strongly-typed konfigurace aplikace
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
builder.Services.AddAttributedServices(typeof(Program));

// authentication
builder.AddCisServiceAuthentication();

// add this service
builder.AddRiskIntegrationService(appConfiguration);

// add BE services
builder.Services
    .AddCodebookService(true)
    .AddCisServiceDiscovery(true);

/*builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});*/

// Swashbuckle.AspNetCore
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "1.0" });
    c.EnableAnnotations(true, true);
    c.UseAllOfForInheritance();
    //c.SchemaGeneratorOptions.SubTypesSelector = SwaggerTools.GetDataContractKnownTypes;
});

builder.Services.AddServiceModelGrpc();
builder.Services.AddServiceModelGrpcSwagger();
builder.Services.AddGrpcReflection();
#endregion register builder

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseCisServiceUserContext();
app.UseCisLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "1.0");
});

// Enable ServiceModel.Grpc HTTP/1.1 JSON gateway for Swagger UI, button "Try it out"
app.UseServiceModelGrpcSwaggerGateway();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.RiskIntegrationService.Api.Services.TestService>();

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