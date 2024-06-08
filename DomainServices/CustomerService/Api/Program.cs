using CIS.Infrastructure.Messaging;
using DomainServices.CustomerService.ExternalServices;
using ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<DomainServices.CustomerService.Api.Configuration.AppConfiguration>()
    .AddErrorCodeMapper(DomainServices.CustomerService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService()
            .AddHouseholdService()
            .AddSalesArrangementService()
            .AddUserService()
            .AddCaseService();
    })
    .Build((builder, appConfiguration) =>
    {
        appConfiguration.Validate();

        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.CustomerManagement.V2.ICustomerManagementClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.Contacts.V1.IContactClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.CustomerProfile.V1.ICustomerProfileClient>();
        builder.AddExternalService<DomainServices.CustomerService.ExternalServices.Kyc.V1.IKycClient>();
        builder.AddExternalService<ExternalServices.MpHome.V1.IMpHomeClient>();

        builder.AddCisMessaging()
               .AddKafkaFlow(msg =>
               {
                   msg.AddConsumerJson(appConfiguration.CustomerManagementEventTopic,
                                       handlers =>
                                       {
                                           handlers.AddHandler<DomainServices.CustomerService.Api.Messaging.PartyCreated.PartyCreatedHandler>();
                                           handlers.AddHandler<DomainServices.CustomerService.Api.Messaging.PartyUpdated.PartyUpdatedHandler>();
                                       });
               });
    })
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<DomainServices.CustomerService.Api.Endpoints.v1.CustomerService>();
    })
    .Run();
