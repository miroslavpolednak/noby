using System.IO.Compression;
using CIS.DomainServicesSecurity;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Validation;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.Notification;
using CIS.InternalServices.NotificationService.Api.Extensions;
using CIS.InternalServices.NotificationService.Api.Messaging;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.S3;
using DomainServices.CodebookService.Abstraction;
using FluentValidation;
using MediatR;
using ProtoBuf.Grpc.Server;

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
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(GrpcValidationBehaviour<,>));

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
builder.Services.AddMessaging(builder.GetKafkaConfiguration());

// s3 client
builder.Services.AddS3Client(builder.GetS3Configuration());


// database
builder.AddEntityFramework<NotificationDbContext>("nobyDb");

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