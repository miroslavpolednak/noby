using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Messaging;
using DomainServices.RealEstateValuationService.ExternalServices;

namespace DomainServices.RealEstateValuationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRealEstateValuationService(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder
            .Configuration
            .GetSection("AppConfiguration")
            .Get<Configuration.AppConfiguration>()
            ?? throw new CisConfigurationNotFound("AppConfiguration");
        appConfiguration.Validate();
        
        // dbcontext
        builder.AddEntityFramework<RealEstateValuationServiceDbContext>();

        builder.AddExternalService<ExternalServices.PreorderService.V1.IPreorderServiceClient>();
        builder.AddExternalService<ExternalServices.LuxpiService.V1.ILuxpiServiceClient>();

        // kafka messaging
        builder.AddCisMessaging()
            .AddKafka(typeof(StartupExtensions).Assembly)
            .AddConsumer<Messaging.CollateralValuationProcessChanged.CollateralValuationProcessChangedConsumer>()
            .AddConsumer<Messaging.InformationRequestProcessChanged.InformationRequestProcessChangedConsumer>()
            .AddConsumerTopicAvro<ISbWorkflowProcessEvent>(appConfiguration.SbWorkflowProcessTopic!)
            .Build();
        
        return builder;
    }
}
