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
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisHealthChecks()
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

// ---------------------------------------------------------------------------------

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseHttpsRedirection();
app.UseServiceDiscovery();

app
    .UseCustomSwagger()
    .UseGrpc2WebApiException()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCisServiceUserContext()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapCisHealthChecks();
        endpoints.MapGrpcService<NotificationService>();
        endpoints.MapCodeFirstGrpcReflectionService();
        endpoints.MapControllers();
    });

await app.RunAsync();