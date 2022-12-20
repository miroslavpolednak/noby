using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.ServiceDiscovery.Api.Endpoints;
using Microsoft.AspNetCore.HttpLogging;
using System.Reflection.PortableExecutable;

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
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program));

// add .NET logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

// add mediatr
builder.Services.AddMediatR(typeof(Program).Assembly);

// health checks
builder.AddCisHealthChecks();
// logging, tracing
builder
    .AddCisLogging()
    .AddCisTracing();

// add general Dapper repository
builder.Services.AddDapper(builder.Configuration.GetConnectionString("default")!);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();
app.UseCisLogging();
app.UseHttpLogging();

app.MapCisHealthChecks();
app.MapGrpcService<DiscoveryService>();
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