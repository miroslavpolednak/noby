using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;
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

        builder.Services.AddHouseholdService()
                    .AddSalesArrangementService()
                    .AddCodebookService()
                    .AddDataAggregatorService()
                    .AddDocumentArchiveService()
                    .AddProductService();
        
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        // sulm
        builder.AddExternalService<ExternalServices.Sulm.V1.ISulmClient>();

        // registrace background jobu
        builder.AddCisBackgroundService<CheckDocumentsArchivedJob>();
        builder.AddCisBackgroundServiceCustomConfiguration<CheckDocumentsArchivedJob, CheckDocumentsArchivedJobConfiguration>();
       
        return builder;
    }
}
