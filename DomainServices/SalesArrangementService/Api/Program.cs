using CIS.Infrastructure.StartupExtensions;
using DomainServices.SalesArrangementService.Api.Endpoints.Maintanance;
using DomainServices.SalesArrangementService.Api.Endpoints.v1;
using ExternalServices;
using SharedComponents.DocumentDataStorage;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddDistributedCache()
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
        builder.AddExternalService<Eas.IEasClient>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.SalesArrangementService.Api.Database.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.DocumentArchiveServiceDbContext>(connectionStringKey: "documentArchiveDb");

        builder.AddDocumentDataStorage();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<SalesArrangementService>();
        app.MapGrpcService<MaintananceService>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}