using System.IO.Compression;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.v1;
using CIS.InternalServices.NotificationService.Api.Extensions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using FluentValidation;
using MediatR;
using ProtoBuf.Grpc.Server;
using DomainServices;
using CIS.InternalServices;
using CIS.InternalServices.NotificationService.Api.Services.Messaging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var winSvc = args.Any(t => t.Equals("winsvc"));
var webAppOptions = winSvc
    ?  new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :  new WebApplicationOptions { Args = args };

var builder = WebApplication.CreateBuilder(webAppOptions);

// Configuration
builder.Configure();

// Mvc
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

// Cis
builder.AddCisEnvironmentConfiguration();
builder
    .AddCisCoreFeatures()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisServiceAuthentication();

builder.Services.AddAttributedServices(typeof(Program));

// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Validators
builder.Services
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.CisMediatR.GrpcValidationBehavior<,>));

builder.Services.Scan(selector => selector
    .FromAssembliesOf(typeof(Program))
    .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

// gRPC
builder.Services
    .AddCodeFirstGrpcReflection()
    .AddCodeFirstGrpc(config =>
    {
        config.ResponseCompressionLevel = CompressionLevel.Optimal;
        config.Interceptors.Add<GenericServerExceptionInterceptor>();
    });

// codebook client
builder.Services.AddCodebookService();

// messaging - kafka consumers and producers
builder.AddMessaging();

// s3 client
builder.AddS3Client();

// smtp
builder.AddSmtpClient();

// database
builder.AddEntityFramework<NotificationDbContext>(connectionStringKey: "nobyDb");

// swagger
builder.AddCustomSwagger();

// kestrel
builder.UseKestrelWithCustomConfiguration();

if (winSvc)
{
    builder.Host.UseWindowsService();
}

builder.Services.AddHealthChecks();
// ---------------------------------------------------------------------------------

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseServiceDiscovery();

app
    .UseCustomSwagger()
    .UseGrpc2WebApiException()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCisServiceUserContext();

app.MapCodeFirstGrpcHealthChecks();
app.MapGrpcService<NotificationService>();
app.MapCodeFirstGrpcReflectionService();
app.MapControllers();
app.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
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