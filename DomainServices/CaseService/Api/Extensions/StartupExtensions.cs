using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api.Database;
using ExternalServices;
using DomainServices.CaseService.ExternalServices;
using Ext1 = ExternalServices;
using Ext2 = DomainServices.CaseService.ExternalServices;
using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.Messaging.Kafka;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCaseService(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder
            .Configuration
            .GetSection("AppConfiguration")
            .Get<Configuration.AppConfiguration>()
            ?? throw new CisConfigurationNotFound("AppConfiguration");
        appConfiguration.Validate();

        // EAS svc
        builder.AddExternalService<Ext1.Eas.V1.IEasClient>();

        // EAS EasSimulationHT svc
        builder.AddExternalService<Ext1.EasSimulationHT.V1.IEasSimulationHTClient>();

        // SB webapi svc
        builder.AddExternalService<Ext2.SbWebApi.V1.ISbWebApiClient>();

        // dbcontext
        builder.AddEntityFramework<CaseServiceDbContext>();

        // pridat distribuovanou cache. casem redis?
        builder.AddCisDistributedCache();

        // kafka messaging
        builder.AddCisMessaging()
            .AddKafka()
            .AddConsumers(t =>
            {
                t.AddConsumer<Messaging.MainLoanProcessChanged.MainLoanProcessChangedConsumer>();
                t.AddConsumer<Messaging.CaseStateChangedProcessingCompleted.CaseStateChanged_ProcessingCompletedConsumer>();
            })
            .AddConsumersToTopic((f, c) =>
            {
                f.AddTopic<IMarker1, Messaging.MainLoanProcessChanged.MainLoanProcessChangedConsumer, cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.MainLoanProcessChanged>(c, appConfiguration.MainLoanProcessChangedTopic!);
                f.AddTopic<IMarker2, Messaging.CaseStateChangedProcessingCompleted.CaseStateChanged_ProcessingCompletedConsumer, cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1.CaseStateChanged_ProcessingCompleted>(c, appConfiguration.CaseStateChangedProcessingCompletedTopic!);
            })
            .Build();

        return builder;
    }
}
