using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

#pragma warning disable CS0436 // Type conflicts with imported type

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<CIS.InternalServices.TaskSchedulingService.Api.Configuration.AppConfiguration>()
    .AddErrorCodeMapper(CIS.InternalServices.TaskSchedulingService.Api.ErrorCodeMapper.Init())
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
        // pridat databazi
        builder.AddDapper();

        // pridat scheduling
        builder.Services.AddSchedulingServices();
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
