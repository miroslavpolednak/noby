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
using ProtoBuf.Grpc.Server;
using DomainServices;
using CIS.InternalServices;
using CIS.InternalServices.NotificationService.Api.ErrorHandling;
using CIS.InternalServices.NotificationService.Api.Services.Messaging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var winSvc = args.Any(t => t.Equals("winsvc"));
var webAppOptions = winSvc
    ?  new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :  new WebApplicationOptions { Args = args };

var builder = WebApplication.CreateBuilder(webAppOptions);

var log = builder.CreateStartupLogger();

try
{
    // Configuration
    builder.Configure();

    // Mvc
    builder.Services
        .AddHsts(options =>
        {
            options.Preload = true;
            options.MaxAge = TimeSpan.FromDays(360);
        })
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressMapClientErrors = true;
            options.AddCustomInvalidModelStateResponseFactory();
        });

    // Cis
    builder.AddCisEnvironmentConfiguration();
    builder
        .AddCisCoreFeatures()
        .AddCisLogging()
        .AddCisTracing()
        .AddCisServiceAuthentication()
        .Services
            .AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init())
            .AddAttributedServices(typeof(Program));

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
    log.ApplicationBuilt();

    app.Use((context, next) =>
    {
        context.Request.EnableBuffering();
        return next();
    });

    app.UseHsts();

    app.UseHttpsRedirection();

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

    log.ApplicationRun();
    app.Run();
}
catch (Exception ex)
{
    log.CatchedException(ex);
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