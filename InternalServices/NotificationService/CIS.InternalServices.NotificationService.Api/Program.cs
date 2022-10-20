using System.IO.Compression;
using CIS.DomainServicesSecurity;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Validation;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.Notification;
using CIS.InternalServices.NotificationService.Api.Extensions;
using CIS.InternalServices.NotificationService.Api.HostedServices;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Msc.AvroSerializers;
using Confluent.Kafka.DependencyInjection;
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

// Codebook service
builder.Services.AddCodebookService();

// kafka
var kafkaConfiguration = builder.GetKafkaConfiguration();

builder.Services
    .AddAvroSerializers()
    .AddKafkaClient<McsBusinessService>(new Dictionary<string, string>
    {
        { "group.id", kafkaConfiguration.GroupId},
        { "bootstrap.servers", kafkaConfiguration.BootstrapServers.Logman },
        { "ssl.keystore.location", kafkaConfiguration.SslKeystoreLocation },
        { "ssl.keystore.password", kafkaConfiguration.SslKeystorePassword },
        { "security.protocol", kafkaConfiguration.SecurityProtocol },
        { "ssl.ca.location", kafkaConfiguration.SslCaLocation },
        { "ssl.certificate.location", kafkaConfiguration.SslCertificateLocation },
        { "debug", kafkaConfiguration.Debug }
    })
    .AddKafkaClient<McsLogmanService>(new Dictionary<string, string>
    {
        { "group.id", kafkaConfiguration.GroupId},
        { "bootstrap.servers", kafkaConfiguration.BootstrapServers.Business },
        { "ssl.keystore.location", kafkaConfiguration.SslKeystoreLocation },
        { "ssl.keystore.password", kafkaConfiguration.SslKeystorePassword },
        { "security.protocol", kafkaConfiguration.SecurityProtocol },
        { "ssl.ca.location", kafkaConfiguration.SslCaLocation },
        { "ssl.certificate.location", kafkaConfiguration.SslCertificateLocation },
        { "debug", kafkaConfiguration.Debug }
    })
    .AddBackgroundServices();

// database
builder.AddEntityFramework<NotificationDbContext>("nobyDb");

// swagger
builder.AddCustomSwagger();

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

if (winSvc)
{
    builder.Host.UseWindowsService();
}

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