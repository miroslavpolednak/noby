using CIS.Infrastructure.StartupExtensions;
using DomainServices.UserService.Api.Database;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddDistributedCache()
    .AddErrorCodeMapper(DomainServices.UserService.Api.ErrorCodeMapper.Init())
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "User Service API";
        options
            .AddOpenApiXmlCommentFromBaseDirectory("DomainServices.UserService.Contracts.xml")
            .AddOpenApiXmlCommentFromBaseDirectory("SharedTypes.xml");
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