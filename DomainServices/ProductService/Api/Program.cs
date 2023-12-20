using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

SharedComponents
    .GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(DomainServices.ProductService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService()
            .AddUserService()
            .AddOfferService()
            .AddSalesArrangementService()
            .AddHouseholdService()
            .AddCaseService();
    })
    .Build(builder =>
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();
        // MpHome svc
        builder.AddExternalService<IMpHomeClient>();
        builder.AddExternalService<DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient>();

        builder.Services.AddDapper(builder.Configuration.GetConnectionString("konsDb") ?? throw new ArgumentException("Missing connection string konsDb."));
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.ProductService.Api.Endpoints.ProductService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}