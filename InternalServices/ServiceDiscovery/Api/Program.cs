using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.ServiceDiscovery.Api;
using CIS.InternalServices.ServiceDiscovery.Api.Endpoints;
using Microsoft.AspNetCore.HttpLogging;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

#region register builder.Services
// globalni nastaveni prostredi
var envConfiguration = builder.AddCisCoreFeatures()
    .AddCisEnvironmentConfiguration();
builder.Services.AddAttributedServices(typeof(Program));

// add .NET logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

// add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// logging, tracing
builder
    .AddCisLogging()
    .AddCisTracing();

// add general Dapper repository
builder.Services.AddDapper(builder.Configuration.GetConnectionString("default")!);

// add GRPC
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GenericServerExceptionInterceptor>();
}).AddJsonTranscoding();
builder.Services
    .AddGrpcReflection()
    .AddServiceDiscoverySwagger();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();
app.UseHttpLogging();

app.MapGrpcReflectionService();
app.MapGlobalHealthChecks();
app.UseServiceDiscoverySwagger();

app.MapGrpcService<DiscoveryService>();

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