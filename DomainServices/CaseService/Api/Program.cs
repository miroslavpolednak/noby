using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api.Database;
using ExternalServices;
using Ext1 = ExternalServices;
using CIS.Infrastructure.Messaging;
using DomainServices.CaseService.Api.Messaging.MessageHandlers;
using DomainServices.CaseService.Api.Endpoints.v1;
using DomainServices.CaseService.Api.Messaging.MortgageServicingBatch;

SharedComponents.GrpcServiceBuilder
                .CreateGrpcService(args, typeof(Program))
                .AddApplicationConfiguration<DomainServices.CaseService.Api.Configuration.AppConfiguration>()
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
                                               .AddHandler<LoanExtraPaymentProcessChangedHandler>()
                                               .AddHandler<LoanRetentionProcessChangedHandler>();
                                   });

                               msg.AddConsumerAvro<CaseStateChangedProcessingCompletedHandler>(appConfiguration.SbWorkflowInputProcessingTopic!);

                               msg.AddBatchConsumerAvro<MortgageServicingBatchHandler>(appConfiguration.MortgageServicingMortgageChangesTopic!);
                           });
                })
                .MapGrpcServices((app, _) =>
                {
                    app.MapGrpcService<CaseService>();
                    app.MapGrpcService<DomainServices.CaseService.Api.Endpoints.Maintanance.MaintananceService>();
                })
                .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}