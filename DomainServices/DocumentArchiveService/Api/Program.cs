using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;
using CIS.Core;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<DomainServices.DocumentArchiveService.Api.Configuration.AppConfiguration>()
    .AddGrpcServiceOptions(options =>
    {
        options.MaxReceiveMessageSize = 25 * 1024 * 1024; // 25 MB
        options.MaxSendMessageSize = 25 * 1024 * 1024; // 25 MB
    })
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "DocumentArchive Service API";
        options.AddOpenApiXmlCommentFromBaseDirectory("DomainServices.DocumentArchiveService.xml");
    })
    .AddRequiredServices(services =>
    {
        services
            .AddUserService();
    })
    .AddErrorCodeMapper(DomainServices.DocumentArchiveService.Api.ErrorCodeMapper.Init())
    .Build((builder, appConfiguration) =>
    {
        if (appConfiguration?.ServiceUser2LoginBinding is null)
            throw new CisConfigurationNotFound(CIS.Core.CisGlobalConstants.DefaultAppConfigurationSectionName);

        builder.AddExternalService<ISdfClient>();

        builder.AddExternalService<IDocumentServiceRepository>();

        builder.Services.AddSingleton<IDocumentMapper, DocumentMapper>();

        // databases
        builder.Services
            .AddDapper<DomainServices.DocumentArchiveService.Api.Database.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey)!);

        // dbcontext
        builder.AddEntityFramework<DomainServices.DocumentArchiveService.Api.Database.DocumentArchiveDbContext>(connectionStringKey: CisGlobalConstants.DefaultConnectionStringKey);
    })
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<DomainServices.DocumentArchiveService.Api.Endpoints.DocumentArchiveServiceGrpc>();
    })
    .Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}