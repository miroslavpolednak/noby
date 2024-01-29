using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog;
using CIS.InternalServices.NotificationService.Api.Services.Messaging;
using CIS.InternalServices.NotificationService.Api.Services.User;
using SharedComponents.DocumentDataStorage;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using SharedComponents.Storage;
using CIS.InternalServices.NotificationService.Api.Legacy.ErrorHandling;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<CIS.InternalServices.NotificationService.Api.Configuration.AppConfiguration>(new CIS.InternalServices.NotificationService.Api.Configuration.AppConfigurationValidator())
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

        #region registrace background jobu
        // odeslani MPSS emailu
        builder.AddCisBackgroundService<SendEmailsJob, SendEmailsJobConfiguration>(new SendEmailsJobConfigurationValidator());

        // zruseni odesilani MPSS emailu po expiraci platnosti
        builder.AddCisBackgroundService<SetExpiredEmailsJob, SetExpiredEmailsJobConfiguration>(new SetExpiredEmailsJobConfigurationValidator());
        #endregion registrace background jobu

        // ukladani payloadu - document data storage
        builder.AddDocumentDataStorage();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<CIS.InternalServices.NotificationService.Api.Endpoints.v2.NotificationServiceV2>();
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