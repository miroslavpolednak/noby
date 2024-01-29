using CIS.Infrastructure.Security;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices;
using CIS.InternalServices.NotificationService.Api;
using CIS.InternalServices.NotificationService.Api.ErrorHandling;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog;
using CIS.InternalServices.NotificationService.Api.Services.Messaging;
using CIS.InternalServices.NotificationService.Api.Services.User;
using CIS.InternalServices.NotificationService.Api.Swagger;
using SharedComponents.DocumentDataStorage;
using CIS.InternalServices.NotificationService.Api.BackgroundServices;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Api.Configuration;
using SharedComponents.Storage;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddUserService()
            .AddCodebookService();
    })
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "Notification Service API";
        options.AddOpenApiXmlCommentFromBaseDirectory("CIS.InternalServices.NotificationService.Contracts.xml");
    })
    .Build(builder =>
    {
        // Configuration
        builder.Configure();

        // storage
        builder
            .AddCisStorageServices()
            .AddStorageClient<IEmailAttachmentStorage>();

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

        // audit logger
        builder.Services
            .AddScoped<ISmsAuditLogger, SmsAuditLogger>();

        // repository
        // entity framework
        builder.AddEntityFramework<NotificationDbContext>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

        // messaging - kafka consumers and producers
        builder.AddMessaging();

        // user
        builder.Services.AddScoped<IUserAdapterService, UserAdapterService>();

        // registrace background jobu
        builder.AddBackroundJobs();

        // ukladani payloadu - document data storage
        builder.AddDocumentDataStorage();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<CIS.InternalServices.NotificationService.Api.Endpoints.UserService>();
    })
    .Run();


   

// swagger
/*builder.AddCustomSwagger();


var app = builder.Build();
log.ApplicationBuilt();

app.UseMiddleware<AuditRequestResponseMiddleware>();

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

//app.MapGrpcService<NotificationService>();
app.MapControllers();*/


#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}