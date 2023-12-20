using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(DomainServices.SalesArrangementService.Api.ErrorCodeMapper.Init())
    .AddRollbackCapability()
    .AddRequiredServices(services =>
    {
        services
            .AddCaseService()
            .AddCodebookService()
            .AddCustomerService()
            .AddOfferService()
            .AddUserService()
            .AddHouseholdService()
            .AddCustomerService()
            .AddDocumentArchiveService()
            .AddDocumentOnSAService()
            .AddRealEstateValuationService()
            .AddDocumentGeneratorService()
            .AddDataAggregatorService()
            .AddRealEstateValuationService()
            .AddProductService();
    })
    .Build(builder =>
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.SalesArrangementService.Api.Database.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.DocumentArchiveServiceDbContext>(connectionStringKey: "documentArchiveDb");

        // background svc
        builder.AddCisBackgroundService<DomainServices.SalesArrangementService.Api.BackgroundServices.OfferGuaranteeDateToCheck.OfferGuaranteeDateToCheckJob>();
        builder.AddCisBackgroundService<DomainServices.SalesArrangementService.Api.BackgroundServices.CancelCase.CancelCaseJob>();
        builder.AddCisBackgroundService<DomainServices.SalesArrangementService.Api.BackgroundServices.CancelServiceSalesArrangement.CancelServiceSalesArrangementJob>();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.SalesArrangementService.Api.Endpoints.SalesArrangementService>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}