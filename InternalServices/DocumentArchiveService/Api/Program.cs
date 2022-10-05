using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.DocumentArchiveService.Api;
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

// authentication
builder.AddCisServiceAuthentication();

// add this service
builder.AddDocumentArchiveService();

// swagger
builder.AddDocumentArchiveSwagger();

// add grpc
builder.AddDocumentArchiveGrpc();
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

    endpoints.MapGrpcService<CIS.InternalServices.DocumentArchiveService.Api.Endpoints.DocumentArchiveServiceGrpc>();

    endpoints.MapCodeFirstGrpcReflectionService();

    endpoints.MapControllers();
});

// swagger
app.UseDocumentArchiveSwagger();

// print gRPC PROTO file
//var schemaGenerator = new ProtoBuf.Grpc.Reflection.SchemaGenerator();
//var proto1 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2.ICreditWorthinessService>();
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