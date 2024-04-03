using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Api.Endpoints.v1;
using ExternalServices;
using SharedComponents.DocumentDataStorage;

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
        builder.AddExternalService<ExternalServices.SbWebApi.V1.ISbWebApiClient>();

        builder.AddDocumentDataStorage();

        // dbcontext
        builder.AddEntityFramework<DomainServices.OfferService.Api.Database.OfferServiceDbContext>();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<OfferService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}