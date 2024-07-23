using CIS.Infrastructure.StartupExtensions;
using ExternalServices;
using ExternalServices.SbQueues;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using ExternalServices.ESignatures.V1;
using CIS.Infrastructure.Messaging;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
	.AddApplicationConfiguration<DomainServices.DocumentOnSAService.Api.Configuration.AppConfiguration>()
	.AddErrorCodeMapper(DomainServices.DocumentOnSAService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddHouseholdService()
            .AddSalesArrangementService()
            .AddCodebookService()
            .AddDataAggregatorService()
            .AddDocumentArchiveService()
            .AddProductService()
            .AddCaseService()
            .AddCustomerService()
            .AddUserService()
            .AddDocumentGeneratorService();
    })
    .Build((builder, appConfiguration) =>
    {
        // EAS svc
        builder.AddExternalService<IEasClient>();

        // sulm
        builder.AddExternalService<ISulmClient>();

        // ePodpisy
        builder.AddExternalService<IESignaturesClient>();

        // ePodpisy fronta
        builder.AddExternalService<ISbQueuesRepository>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.DocumentOnSAService.Api.Database.DocumentOnSAServiceDbContext>();

		builder
            .AddCisMessaging()
            .AddKafkaFlow(msg =>
            {
                msg.AddConsumerAvro<DomainServices.DocumentOnSAService.Api.Messaging.DocumentStateChanged.DocumentStateChangedHandler>(appConfiguration.ESignatureDocumentStateChangedTopic);
			});
    })
    .MapGrpcServices((app, appConfiguration) =>
    {
        app.MapGrpcService<DomainServices.DocumentOnSAService.Api.Endpoints.DocumentOnSAServiceGrpc>();
        app.MapGrpcService<DomainServices.DocumentOnSAService.Api.Endpoints.MaintananceService>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
