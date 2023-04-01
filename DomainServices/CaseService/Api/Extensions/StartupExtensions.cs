using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api.Database;
using ExternalServices;
using DomainServices.CaseService.ExternalServices;
using Ext1 = ExternalServices;
using Ext2 = DomainServices.CaseService.ExternalServices;
using CIS.Infrastructure.Messaging;
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
            .AddConsumer<Messaging.MainLoanProcessChanged.MainLoanProcessChangedConsumer>()
            .AddConsumer<Messaging.CaseStateChangedProcessingCompleted.CaseStateChanged_ProcessingCompletedConsumer>()
            .AddConsumerTopic<IMarker1>(appConfiguration.MainLoanProcessChangedTopic!)
            .AddConsumerTopic<IMarker2>(appConfiguration.CaseStateChangedProcessingCompletedTopic!)
            .Build();

        return builder;
    }
}
