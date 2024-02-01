using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CustomerService.ExternalServices;
using ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<DomainServices.CustomerService.Api.Configuration.AppConfiguration>()
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService()
            .AddHouseholdService()
            .AddSalesArrangementService()
            .AddCaseService();
    })
    .Build((builder, appConfiguration) =>
    {
        appConfiguration.Validate();

        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb")!);

        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.CustomerManagement.V2.ICustomerManagementClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.Contacts.V1.IContactClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.CustomerProfile.V1.ICustomerProfileClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.Kyc.V1.IKycClient>();
        builder.AddExternalService<ExternalServices.MpHome.V1.IMpHomeClient>();

        builder.AddCisMessaging()
            .AddKafka()
            .AddConsumer<DomainServices.CustomerService.Api.Messaging.PartyCreated.PartyCreatedConsumer>()
            .AddConsumer<DomainServices.CustomerService.Api.Messaging.PartyUpdated.PartyUpdatedConsumer>()
            .AddConsumerTopicJson<DomainServices.CustomerService.Api.Messaging.Abstraction.ICustomerManagementEvent>(appConfiguration.CustomerManagementEventTopic)
            .Build();
    })
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<DomainServices.CustomerService.Api.Endpoints.CustomerService>();
    })
    .Run();
