using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(DomainServices.OfferService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddRiskIntegrationService()
            .AddCodebookService()
            .AddUserService();
    })
    .Build(builder =>
    {
        // EAS EasSimulationHT svc
        builder.AddExternalService<ExternalServices.EasSimulationHT.V1.IEasSimulationHTClient>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.OfferService.Api.Database.OfferServiceDbContext>();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.OfferService.Api.Endpoints.OfferService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}