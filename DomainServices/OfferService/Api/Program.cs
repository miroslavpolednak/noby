using CIS.Infrastructure.Messaging;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Api.Endpoints.v1;
using DomainServices.OfferService.Api.Messaging.LoanRetentionProcessChanged;
using ExternalServices;
using SharedComponents.DocumentDataStorage;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddDistributedCache()
    .AddApplicationConfiguration<DomainServices.OfferService.Api.Configuration.AppConfiguration>()
    .AddErrorCodeMapper(DomainServices.OfferService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddCaseService()
            .AddRiskIntegrationService()
            .AddCodebookService()
            .AddUserService();
    })
    .Build((builder, appConfig) =>
    {
        appConfig.Validate();

        // EAS EasSimulationHT svc
        builder.AddExternalService<EasSimulationHT.IEasSimulationHTClient>();
        builder.AddExternalService<ExternalServices.SbWebApi.V1.ISbWebApiClient>();
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        builder.AddDocumentDataStorage();

        // dbcontext
        builder.AddEntityFramework<DomainServices.OfferService.Api.Database.OfferServiceDbContext>();

        builder.AddCisMessaging()
               .AddKafkaFlow(msg =>
               {
                   msg.AddConsumerAvro<LoanRetentionProcessChangedHandler>(appConfig.SbWorkflowProcessTopic!);
               });
    })
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<OfferService>();
        app.MapGrpcService<DomainServices.OfferService.Api.Endpoints.MaintananceService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}