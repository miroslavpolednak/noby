using CIS.DomainServicesSecurity;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Endpoints.Notification;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisHealthChecks()
    .AddCisServiceAuthentication();

builder.Services.AddAttributedServices(typeof(Program));

// Add services to the container.
builder.Services.AddGrpc();

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// if (runAsWinSvc) builder.Host.UseWindowsService();

var app = builder.Build();

app
    .UseGrpc2WebApiException()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapCisHealthChecks();
        endpoints.MapGrpcService<NotificationService>();
        endpoints.MapCodeFirstGrpcReflectionService();
        endpoints.MapControllers();
    });

app.Run();
