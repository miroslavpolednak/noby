using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using DomainServices.RiskIntegrationService.Api;
using CIS.Infrastructure.Security;
using ProtoBuf.Grpc.Server;
using CIS.InternalServices;
using DomainServices;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

AppConfiguration appConfiguration = new();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);

#region register builder
// strongly-typed konfigurace aplikace
builder.Services.AddSingleton(appConfiguration);

// globalni nastaveni prostredi
builder
    .AddCisCoreFeatures()
    .AddCisEnvironmentConfiguration();

// logging 
builder
    .AddCisLogging()
    .AddCisTracing();

// health checks
builder.Services.AddAttributedServices(typeof(Program));

// authentication
builder.AddCisServiceAuthentication();

// add this service
builder.AddRipService();

// add BE services
builder.Services
    .AddCodebookService()
    .AddCisServiceDiscovery();

// swagger
builder.AddRipSwagger();

// add grpc
builder.AddRipGrpc();
builder.Services.AddHealthChecks();
#endregion register builder

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseServiceDiscovery();

// zachytavat vyjimky pri WebApi volani a transformovat je do 400 bad request
app.UseGrpc2WebApiException();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();

app.MapCodeFirstGrpcHealthChecks();
app.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2.CustomersExposureService>();
app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.RiskBusinessCaseService>();
app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.CreditWorthinessService>();
app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.LoanApplicationService>();
app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.TestServiceGrpc>();

app.MapCodeFirstGrpcReflectionService();

app.MapControllers();

// swagger
app.UseRipSwagger();

// print gRPC PROTO file
//var schemaGenerator = new ProtoBuf.Grpc.Reflection.SchemaGenerator();
//var proto1 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.ILoanApplicationService>();
//var proto1 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2.ICreditWorthinessService>();
//var proto1 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2.IRiskBusinessCaseService>();
//File.WriteAllText("d:\\proto1.proto", proto1);

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