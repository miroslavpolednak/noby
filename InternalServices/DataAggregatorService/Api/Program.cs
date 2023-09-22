using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DataAggregatorService.Api.Configuration;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<CIS.InternalServices.DataAggregatorService.Api.Configuration.AppConfiguration>()
    .AddCustomServices((builder, appConfiguration) =>
    {
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("default")!);

        if (appConfiguration.UseCacheForConfiguration)
        {
            builder.Services.AddMemoryCache();
            builder.Services.Decorate<IConfigurationManager, CachedConfigurationManager>();
        }
    })
    .AddRequiredServices(services =>
    {
        services
            .AddCodebookService()
            .AddSalesArrangementService()
            .AddCaseService()
            .AddOfferService()
            .AddUserService()
            .AddCustomerService()
            .AddProductService()
            .AddHouseholdService()
            .AddDocumentOnSAService();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<CIS.InternalServices.DataAggregatorService.Api.Endpoints.DataAggregatorServiceGrpc>();
    })
    .Run();

namespace CIS.InternalServices.DataAggregatorService.Api
{
    public partial class Program
    {
        // Expose the Program class for use with WebApplicationFactory<T>
    }
}
