using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Api.BackgroundServiceJobs.CheckDocumentsArchived;
using DomainServices.DocumentOnSAService.Api.Configuration;
using ExternalServices;

namespace DomainServices.DocumentOnSAService.Api.Extensions;

internal static class StartupExtensions
{
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
    }

    public static WebApplicationBuilder AddDocumentOnSAServiceService(this WebApplicationBuilder builder)
    {
        builder.Services.AddAttributedServices(typeof(Program));

        // dbcontext
        builder.AddEntityFramework<Database.DocumentOnSAServiceDbContext>(connectionStringKey: "default");

        builder.Services.AddHouseholdService();

        builder.Services.AddSalesArrangementService();

        builder.Services.AddCaseService();

        builder.Services.AddCodebookService();

        builder.Services.AddDataAggregatorService();

        builder.Services.AddDocumentArchiveService();

        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        builder.AddCisPeriodicJob<CheckDocumentsArchivedJob, CheckDocumentsArchivedJobConfiguration>();
       
        return builder;
    }
}
