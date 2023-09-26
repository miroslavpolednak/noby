using CIS.Infrastructure.StartupExtensions;
using DomainServices.UserService.Api.Database;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddRollbackCapability()
    
    .AddDistributedCache()
    .AddErrorCodeMapper(DomainServices.UserService.Api.ErrorCodeMapper.Init())
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "User Service API";
        options
            .AddOpenApiXmlComment("DomainServices.UserService.Contracts.xml")
            .AddOpenApiXmlComment("SharedTypes.GrpcTypes.xml");
    })
    .Build(builder =>
    {
        // dbcontext
        builder.AddEntityFramework<UserServiceDbContext>();
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