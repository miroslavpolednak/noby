using System.Configuration;
using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.SalesArrangementService.Api.Messaging;
using DomainServices.SalesArrangementService.Api.Messaging.Abstraction;
using ExternalServices;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder.Configuration
            .GetSection("AppConfiguration")
            .Get<Configuration.AppConfiguration>()
            ?? throw new CisConfigurationNotFound("AppConfiguration");
        
        appConfiguration.Validate();
        
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        // dbcontext
        builder.AddEntityFramework<Database.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Database.DocumentArchiveService.DocumentArchiveServiceDbContext>(connectionStringKey: "documentArchiveDb");

        // background svc
        builder.AddCisBackgroundService<BackgroundServices.OfferGuaranteeDateToCheck.OfferGuaranteeDateToCheckJob>();

        builder.AddCisMessaging()
            .AddKafka()
            .AddConsumer<WithdrawalProcessConsumer>()
            .AddConsumerTopicJson<IStarbuildWorkflowProcessEvent>(appConfiguration.StarbuildWorkflowProcessEventTopic)
            .Build();
        
        return builder;
    }
}
