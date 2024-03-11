using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api.Database;
using ExternalServices;
using Ext1 = ExternalServices;
using CIS.Infrastructure.Messaging;
using DomainServices.CaseService.Api.Messaging.MessageHandlers;

SharedComponents.GrpcServiceBuilder
                .CreateGrpcService(args, typeof(Program))
                .AddApplicationConfiguration<DomainServices.CaseService.Api.Configuration.AppConfiguration>()
                .AddRollbackCapability()
                .AddDistributedCache()
                .AddErrorCodeMapper(DomainServices.CaseService.Api.ErrorCodeMapper.Init())
                .AddRequiredServices(services =>
                {
                    services
                        .AddRiskIntegrationService()
                        .AddSalesArrangementService()
                        .AddDocumentOnSAService()
                        .AddCodebookService()
                        .AddHouseholdService()
                        .AddProductService()
                        .AddUserService()
                        .AddDocumentOnSAService();
                })
                .Build((builder, appConfiguration) =>
                {
                    appConfiguration.Validate();

                    builder.Services.AddControllers();

                    // EAS svc
                    builder.AddExternalService<Ext1.Eas.V1.IEasClient>();

        // SB webapi svc
        builder.AddExternalService<Ext1.SbWebApi.V1.ISbWebApiClient>();

                    // dbcontext
                    builder.AddEntityFramework<CaseServiceDbContext>();

                    // kafka messaging
                    builder.AddCisMessaging()
                           .AddKafkaFlow(msg =>
                           {
                               msg.AddConsumerAvro(
                                   appConfiguration.SbWorkflowProcessTopic!,
                                   handlers =>
                                   {
                                       handlers.AddHandler<CollateralValuationProcessChangedHandler>()
                                               .AddHandler<ConsultationRequestProcessChangedHandler>()
                                               .AddHandler<IndividualPricingProcessChangedHandler>()
                                               .AddHandler<InformationRequestProcessChangedHandler>()
                                               .AddHandler<MainLoanProcessChangedHandler>()
                                               .AddHandler<WithdrawalProcessChangedHandler>()
                                               .AddHandler<LoanRetentionProcessChangedHandler>();
                                   });

                               msg.AddConsumerAvro<CaseStateChangedProcessingCompletedHandler>(appConfiguration.SbWorkflowInputProcessingTopic!);

                               msg.AddConsumerAvro(
                                   appConfiguration.MortgageServicingMortgageChangesTopic!,
                                   handlers =>
                                   {
                                       handlers.AddHandler<MortgageInstanceChangedHandler>()
                                               .AddHandler<MortgageApplicationChangedHandler>();
                                   });
                           });
                })
                .MapGrpcServices((app, _) =>
                {
                    app.MapGrpcService<DomainServices.CaseService.Api.Endpoints.CaseService>();
                    app.MapGrpcService<DomainServices.CaseService.Api.Endpoints.Maintanance.MaintananceService>();
                })
                .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}