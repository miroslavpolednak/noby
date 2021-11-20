using CIS.Infrastructure.StartupExtensions;

namespace DomainServices.CodebookService.Endpoints.MyTestCodebook;

internal sealed class MyTestCodebookStartup : ICodebookServiceEndpointStartup
{
    public void Register(WebApplicationBuilder builder)
    {
        // register strongly type custom configuration
        MyTestCodebookConfiguration configuration = new();
        builder.Configuration.GetSection("EndpointsConfiguration:MyTestCodebook").Bind(configuration);
        builder.Services.AddSingleton(configuration);

        // register Dapper with custom connection string
        builder.Services.AddDapper<MyTestCodebookRepository>(builder.Configuration.GetSection("EndpointsConfiguration:MyTestCodebook").GetConnectionString("DbConnectionString"));
    }
}
