using CIS.Core;
using CIS.Infrastructure.StartupExtensions;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<CIS.InternalServices.TaskSchedulingService.Api.Configuration.AppConfiguration>()
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
        
    })
    .MapGrpcServices((app, _) =>
    {
        app.MapGrpcService<CIS.InternalServices.TaskSchedulingService.Api.Endpoints.TaskSchedulingService>();
    })
    .Run();

namespace CIS.InternalServices.TaskSchedulingService.Api
{
    public partial class Program
    {
        // Expose the Program class for use with WebApplicationFactory<T>
    }
}
