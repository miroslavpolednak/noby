using CIS.Infrastructure.StartupExtensions;
using ExternalServices;
using ExternalServices.SbQueues;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using ExternalServices.ESignatures.V1;
using DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddErrorCodeMapper(DomainServices.DocumentOnSAService.Api.ErrorCodeMapper.Init())
    .AddRequiredServices(services =>
    {
        services
            .AddHouseholdService()
            .AddSalesArrangementService()
            .AddCodebookService()
            .AddDataAggregatorService()
            .AddDocumentArchiveService()
            .AddProductService()
            .AddCaseService()
            .AddCustomerService()
            .AddUserService()
            .AddDocumentGeneratorService();
    })
    .Build(builder =>
    {
        // EAS svc
        builder.AddExternalService<IEasClient>();

        // sulm
        builder.AddExternalService<ISulmClient>();

        // ePodpisy
        builder.AddExternalService<IESignaturesClient>();

        // ePodpisy fronta
        builder.AddExternalService<ISbQueuesRepository>();

        // dbcontext
        builder.AddEntityFramework<DomainServices.DocumentOnSAService.Api.Database.DocumentOnSAServiceDbContext>();

        bgServices(builder);
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.DocumentOnSAService.Api.Endpoints.DocumentOnSAServiceGrpc>();
        app.MapGrpcService<DomainServices.DocumentOnSAService.Api.Endpoints.MaintananceService>();
    })
    .Run();

[Obsolete("Odstranit po nasazeni scheduling service")]
void bgServices(WebApplicationBuilder builder)
{
    builder.AddCisBackgroundService<CheckDocumentsArchivedJob>();
    builder.AddCisBackgroundService<CheckDocumentsArchivedJob, CheckDocumentsArchivedJobConfiguration>();
}

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
