using CIS.Core;
using CIS.Infrastructure.StartupExtensions;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddDistributedCache()
    .AddErrorCodeMapper(DomainServices.UserService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services.AddUserService();
    })
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "User Service API";
        options
            .AddOpenApiXmlCommentFromBaseDirectory("DomainServices.UserService.Contracts.xml")
            .AddOpenApiXmlCommentFromBaseDirectory("SharedTypes.xml");
    })
    .Build(builder =>
    {
        builder.Services
            .AddDapper(builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey)!);
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.UserService.Api.Endpoints.UserService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}