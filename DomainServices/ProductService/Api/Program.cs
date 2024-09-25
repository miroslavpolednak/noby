using CIS.Core;
using ExternalServices;

SharedComponents
    .GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddDistributedCache()
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

        var pcpCurrentVersion = builder.Configuration[$"{CisGlobalConstants.ExternalServicesConfigurationSectionName}:{DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.ServiceName}:VersionInUse"];

        if (pcpCurrentVersion == DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version)
            builder.AddExternalService<DomainServices.ProductService.ExternalServices.Pcp.IPcpClient>();
        else if (pcpCurrentVersion == DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version2)
            builder.AddExternalServiceV2<DomainServices.ProductService.ExternalServices.Pcp.IPcpClient>();
        else
            builder.AddExternalServiceV3<DomainServices.ProductService.ExternalServices.Pcp.IPcpClient>();
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