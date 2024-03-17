using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api;
using SharedComponents.DocumentDataStorage;
using SharedComponents.Storage;
using CIS.InternalServices.NotificationService.Api.Legacy.ErrorHandling;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.Infrastructure.Messaging;
using CIS.InternalServices.NotificationService.Api.Configuration;
using Microsoft.FeatureManagement;

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
        options.OpenApiVersion = "v2";
        options.OpenApiEndpointVersion = "2.0";
        options.AddOpenApiXmlCommentFromBaseDirectory("CIS.InternalServices.NotificationService.Contracts.xml");
    })
    .Build((builder, configuration) =>
    {
        // file storage
        builder
            .AddCisStorageServices()
            .AddStorageClient<IMcsStorage>();

        // entity framework
        builder.AddEntityFramework<NotificationDbContext>();

        // ukladani payloadu - document data storage
        builder.AddDocumentDataStorage();

        // messaging - kafka consumers and producers
        addMessaging(builder, configuration);

        #region registrace background jobu
        // odeslani MPSS emailu
        builder.AddCisBackgroundService<SendEmailsJob, SendEmailsJobConfiguration>(new SendEmailsJobConfigurationValidator());

        // zruseni odesilani MPSS emailu po expiraci platnosti
        builder.AddCisBackgroundService<SetExpiredEmailsJob, SetExpiredEmailsJobConfiguration>(new SetExpiredEmailsJobConfigurationValidator());
        #endregion registrace background jobu

        #region legacy code
        bool enableLegacyEndpoints = builder.Configuration.GetValue<bool>("FeatureManagement:LegacyEndpoints", false);
        if (enableLegacyEndpoints)
        {
            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.AddCustomInvalidModelStateResponseFactory();
                });
        }

        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.Abstraction.ISmsAuditLogger, CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.SmsAuditLogger>();
        builder.Services.AddScoped<CIS.InternalServices.NotificationService.Api.Services.User.Abstraction.IUserAdapterService, CIS.InternalServices.NotificationService.Api.Services.User.UserAdapterService>();
        builder.AddS3Client();
        #endregion legacy code

    })
    #region legacy code
    .UseMiddlewares((app, _) =>
    {
        var manager = app.Services.GetRequiredService<IFeatureManager>();
        if (manager.IsEnabledAsync("LegacyEndpoints").GetAwaiter().GetResult())
        {
            app.UseWhen(x => x.Request.Path.StartsWithSegments("/v1"), app2 =>
            {
                app2.UseMiddleware<AuditRequestResponseMiddleware>();
            });
            app.MapWhen(x => x.Request.Path.StartsWithSegments("/v1"), app2 =>
            {
                app2.UseEndpoints(t => t.MapControllers());
            });
        }
    })
    #endregion legacy code
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<CIS.InternalServices.NotificationService.Api.Endpoints.v2.NotificationService>();
    })
    .Run();

static void addMessaging(WebApplicationBuilder builder, AppConfiguration configuration)
{
    var kafkaConfiguration = builder
        .AddCisMessaging()
        .AddKafka()
            // Mcs
            .AddConsumer<CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result.McsResultConsumer>()
            .AddConsumerTopicAvro<CIS.InternalServices.NotificationService.Api.Messaging.Messages.Partials.IMcsResultTopic>(configuration.KafkaTopics.McsResult)
            .AddProducerAvro<CIS.InternalServices.NotificationService.Api.Messaging.Messages.Partials.IMcsSenderTopic>(configuration.KafkaTopics.McsSender)
        .Build();

    if (kafkaConfiguration.Disabled)
    {
        builder.Services
            .AddScoped<CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsEmailProducer>(_ => default(CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsEmailProducer))
            .AddScoped<CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsSmsProducer>(_ => default(CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsSmsProducer));
    }
    else
    {
        builder.Services
            .AddScoped<CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsEmailProducer, CIS.InternalServices.NotificationService.Api.Messaging.Producers.McsEmailProducer>()
            .AddScoped<CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction.IMcsSmsProducer, CIS.InternalServices.NotificationService.Api.Messaging.Producers.McsSmsProducer>();
    }
}

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}