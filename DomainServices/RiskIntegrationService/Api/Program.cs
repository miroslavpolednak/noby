using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CodebookService.Abstraction;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Infrastructure.Telemetry;
using DomainServices.RiskIntegrationService.Api;
using CIS.DomainServicesSecurity;
using ProtoBuf.Grpc.Server;

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
builder.AddRipService();

// add BE services
builder.Services
    .AddCodebookService()
    .AddCisServiceDiscovery();

// swagger
builder.AddRipSwagger();

// add grpc
builder.AddRipGrpc();
#endregion register builder

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

// zachytavat vyjimky pri WebApi volani a transformovat je do 400 bad request
app.UseGrpc2WebApiException();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();
app.UseCisLogging();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.CustomersExposureService>();
    endpoints.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.RiskBusinessCaseService>();
    endpoints.MapGrpcService< DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.CreditWorthinessService>();
    endpoints.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.LoanApplicationService>();
    endpoints.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.TestServiceGrpc>();

    endpoints.MapCodeFirstGrpcReflectionService();

    endpoints.MapControllers();
});

// swagger
app.UseRipSwagger();

// print gRPC PROTO file
//var schemaGenerator = new ProtoBuf.Grpc.Reflection.SchemaGenerator();
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