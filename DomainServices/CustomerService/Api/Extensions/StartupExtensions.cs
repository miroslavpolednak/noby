using CIS.Core.Exceptions;
using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CustomerService.Api.Messaging.Abstraction;
using DomainServices.CustomerService.Api.Messaging.PartyCreated;
using DomainServices.CustomerService.Api.Messaging.PartyUpdated;
using DomainServices.CustomerService.ExternalServices;

namespace DomainServices.CustomerService.Api.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomerService(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder.Configuration
            .GetSection("AppConfiguration")
            .Get<Configuration.AppConfiguration>()
            ?? throw new CisConfigurationNotFound("AppConfiguration");
        
        appConfiguration.Validate();
        
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb")!);

        builder.AddExternalService<ExternalServices.CustomerManagement.V1.ICustomerManagementClient>();
        builder.AddExternalService<ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();
        builder.AddExternalService<ExternalServices.CustomerProfile.V1.ICustomerProfileClient>();
        builder.AddExternalService<ExternalServices.Kyc.V1.IKycClient>();

        // CodebookService
        builder.Services
            .AddCodebookService()
            .AddHouseholdService()
            .AddSalesArrangementService()
            .AddCaseService();

        builder.AddCisMessaging()
            .AddKafka()
            .AddConsumer<PartyCreatedConsumer>()
            .AddConsumer<PartyUpdatedConsumer>()
            .AddConsumerTopicJson<ICustomerManagementEvent>(appConfiguration.CustomerManagementEventTopic)
            .Build();
        
        return builder;
    }
}
