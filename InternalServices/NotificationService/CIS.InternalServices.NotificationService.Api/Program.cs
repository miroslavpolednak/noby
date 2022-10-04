using System.IO.Compression;
using CIS.DomainServicesSecurity;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Validation;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Endpoints.Notification;
using CIS.InternalServices.NotificationService.Api.Extensions;
using FluentValidation;
using MediatR;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

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

// swagger
builder.AddCustomSwagger();

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// if (runAsWinSvc) builder.Host.UseWindowsService();

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
