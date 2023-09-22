using CIS.Infrastructure.StartupExtensions;
using DomainServices.UserService.Api.Database;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddRollbackCapability()
    .AddCustomServices(builder =>
    {
        // dbcontext
        builder.AddEntityFramework<UserServiceDbContext>();
    })
    .AddDistributedCache()
    .AddErrorCodeMapper(DomainServices.UserService.Api.ErrorCodeMapper.Init())
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "User Service API";
        options
            .AddOpenApiXmlComment(Path.Combine(AppContext.BaseDirectory, "DomainServices.UserService.Contracts.xml"))
            .AddOpenApiXmlComment(Path.Combine(AppContext.BaseDirectory, "CIS.Infrastructure.gRPC.CisTypes.xml"));
    })
    .SkipRequiredServices()
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.UserService.Api.Endpoints.UserService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}