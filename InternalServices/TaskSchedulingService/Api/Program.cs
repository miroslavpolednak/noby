using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

#pragma warning disable CS0436 // Type conflicts with imported type

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
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
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "TaskSchedulingService API";
        options
            .AddOpenApiXmlCommentFromBaseDirectory("CIS.InternalServices.TaskSchedulingService.Contracts.xml");
    })
    .Build(builder =>
    {
        // pridat databazi
        builder.AddDapper();

        // pridat scheduling
        builder.Services.AddSchedulingServices();

        // zaregistrovat joby
        builder.Services.Scan(selector => selector
            .FromAssembliesOf(typeof(Program))
            .AddClasses(x => x.AssignableTo(typeof(IJob)))
            .AsSelf()
            .WithScopedLifetime());
    })
    .MapGrpcServices(app =>
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