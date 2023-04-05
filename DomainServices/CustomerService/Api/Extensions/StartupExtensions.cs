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
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb")!);

        builder.AddExternalService<ExternalServices.CustomerManagement.V1.ICustomerManagementClient>();
        builder.AddExternalService<ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();
        builder.AddExternalService<ExternalServices.CustomerProfile.V1.ICustomerProfileClient>();
        builder.AddExternalService<ExternalServices.Kyc.V1.IKycClient>();

        // CodebookService
        builder.Services.AddCodebookService();

        builder.AddCisMessaging()
            .AddKafka()
            .AddConsumer<PartyCreatedConsumer>()
            .AddConsumer<PartyUpdatedConsumer>()
            .AddConsumerTopic<ICustomerManagementEvent>("")
            .Build();
        
        return builder;
    }
}
