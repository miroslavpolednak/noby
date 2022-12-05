using CIS.DomainServicesSecurity;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.DocumentGeneratorService.Api;
using CIS.InternalServices.DocumentGeneratorService.Api.Services;
using DomainServices.CodebookService.Clients;

var runAsWinSvc = args.Any(t => t.Equals("winsvc"));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = runAsWinSvc ? AppContext.BaseDirectory : default
});

builder.AddCisEnvironmentConfiguration() // globalni nastaveni prostredi
       .AddCisCoreFeatures()
       .AddCisLogging()
       .AddCisTracing()
       .AddCisHealthChecks()
       .AddCisServiceAuthentication();

builder.Services.Configure<GeneratorConfiguration>(builder.Configuration.GetRequiredSection("GeneratorConfiguration"));

builder.Services.AddCodebookService();

builder.Services.AddAttributedServices(typeof(Program));

builder.Services.AddGrpc(opts => opts.Interceptors.Add<GenericServerExceptionInterceptor>());
builder.Services.AddGrpcReflection();

if (runAsWinSvc) builder.Host.UseWindowsService();

var app = builder.UseKestrelWithCustomConfiguration().Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCisLogging();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DocumentGeneratorService>();

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