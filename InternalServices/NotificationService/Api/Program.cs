using System.IO.Compression;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.v1;
using CIS.InternalServices.NotificationService.Api.Extensions;
using CIS.InternalServices.NotificationService.Api.Services.Mcs;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using DomainServices.CodebookService.Clients;
using FluentValidation;
using MediatR;
using ProtoBuf.Grpc.Server;
using DomainServices;
using CIS.InternalServices;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using KB.Speed.Tracing.Instrumentations.AspNetCore;
using KB.Speed.Tracing.Instrumentations.HttpClient;

var winSvc = args.Any(t => t.Equals("winsvc"));
var webAppOptions = winSvc
    ?  new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :  new WebApplicationOptions { Args = args };

var builder = WebApplication.CreateBuilder(webAppOptions);

// Configuration
builder.Configure();

// Mvc
builder.Services.AddControllers();

// Cis
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisHealthChecks()
    .AddCisServiceAuthentication();

builder.Services.AddAttributedServices(typeof(Program));

// Mediator
builder.Services.AddMediatR(typeof(Program).Assembly);

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
builder.Services.AddSpeedTracing(builder.Configuration, providerBuilder =>
{
    providerBuilder.SetDefaultResourceBuilder()
        .AddDefaultExporter()
        .AddSpeedAspNetInstrumentation()
        .AddSpeedHttpClientInstrumentation()
        .AddMassTransitInstrumentation();
});

builder.Services.AddMessaging(builder.GetKafkaConfiguration());

// s3 client
builder.Services.AddS3Client(builder.GetS3Configuration());

// smtp
builder.Services.AddSmtpClient(builder.GetSmtpConfiguration());

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

// ---------------------------------------------------------------------------------

var app = builder.Build();

app.UseHttpsRedirection();
app.UseServiceDiscovery();

app
    .UseCustomSwagger()
    .UseGrpc2WebApiException()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCisServiceUserContext()
    .UseCisLogging()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapCisHealthChecks();
        endpoints.MapGrpcService<NotificationService>();
        endpoints.MapCodeFirstGrpcReflectionService();
        endpoints.MapControllers();
    });

await app.RunAsync();