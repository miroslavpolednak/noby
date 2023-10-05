using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(DomainServices.HouseholdService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddCaseService()
            .AddCodebookService()
            .AddOfferService()
            .AddSalesArrangementService()
            .AddCustomerService()
            .AddUserService()
            .AddDocumentOnSAService();
    })
    .Build(builder =>
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();
        // sulm
        builder.AddExternalService<ExternalServices.Sulm.V1.ISulmClient>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.HouseholdService.Api.Database.HouseholdServiceDbContext>();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.HouseholdService>();
        app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.CustomerOnSAService>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}