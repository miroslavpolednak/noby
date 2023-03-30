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
                t.AddConsumer<Messaging.SendEmail.SendEmailConsumer>();
            })
            .AddConsumersToTopic((f, c) =>
            {
                f.AddTopic<IMarker1>(c, "NOBY_DS-PERF_MCS_mock_result-event-priv", "example-multiple-type-consumer");
            });

        return builder;
    }
}
