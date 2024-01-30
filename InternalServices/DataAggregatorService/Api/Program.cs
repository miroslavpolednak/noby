using CIS.Core;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DataAggregatorService.Api.Configuration;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<AppConfiguration>()
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
    .Build((builder, appConfiguration) =>
    {
        builder.Services.AddDapper(builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey)!);

        if (appConfiguration.UseCacheForConfiguration)
        {
            builder.Services.AddMemoryCache();
            builder.Services.Decorate<IServiceConfigurationManager, CachedConfigurationManager>();
        }
    })
    .MapGrpcServices((app, _) =>
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
