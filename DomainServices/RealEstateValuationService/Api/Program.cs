using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Messaging;
using DomainServices.RealEstateValuationService.ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<DomainServices.RealEstateValuationService.Api.Configuration.AppConfiguration>()
    .AddGrpcServiceOptions(options =>
    {
        options.MaxReceiveMessageSize = 25 * 1024 * 1024; // 25 MB
    })
    .AddErrorCodeMapper(DomainServices.RealEstateValuationService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService()
            .AddUserService()
            .AddProductService()
            .AddSalesArrangementService()
            .AddCustomerService()
            .AddCaseService()
            .AddOfferService();
    })
    .Build((builder, appConfiguration) =>
    {
        appConfiguration.Validate();

        // dbcontext
        builder.AddEntityFramework<RealEstateValuationServiceDbContext>();

        builder.AddExternalService<DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1.IPreorderServiceClient>();
        builder.AddExternalService<DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1.ILuxpiServiceClient>();

        // kafka messaging
        builder.AddCisMessaging()
            .AddKafka(typeof(Program).Assembly)
            .AddConsumer<DomainServices.RealEstateValuationService.Api.Messaging.CollateralValuationProcessChanged.CollateralValuationProcessChangedConsumer>()
            .AddConsumer<DomainServices.RealEstateValuationService.Api.Messaging.InformationRequestProcessChanged.InformationRequestProcessChangedConsumer>()
            .AddConsumerTopicAvro<ISbWorkflowProcessEvent>(appConfiguration.SbWorkflowProcessTopic!)
            .Build();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.RealEstateValuationService.Api.Endpoints.RealEstateValuationService>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}