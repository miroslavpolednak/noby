using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Messaging;
using DomainServices.RealEstateValuationService.ExternalServices;
using SharedComponents.DocumentDataStorage;

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

        builder.AddDocumentDataStorage();

        // dbcontext
        builder.AddEntityFramework<RealEstateValuationServiceDbContext>();

        builder.AddExternalService<DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1.IPreorderServiceClient>();
        builder.AddExternalService<DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1.ILuxpiServiceClient>();

        builder.AddCisMessaging()
               .AddKafkaFlow(msg =>
               {
                   msg.AddConsumerAvro(appConfiguration.SbWorkflowProcessTopic!,
                                       handlers =>
                                       {
                                           handlers.AddHandler<DomainServices.RealEstateValuationService.Api.Messaging.CollateralValuationProcessChanged.CollateralValuationProcessHandler>();
                                           handlers.AddHandler<DomainServices.RealEstateValuationService.Api.Messaging.InformationRequestProcessChanged.InformationRequestProcessChangedHandler>();
                                       });
               });
    })
    .MapGrpcServices((app, _) =>
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