using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Api.BackgroundServices.UpdateDocumentStatus;
using DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;
using DomainServices.DocumentOnSAService.Api.Configuration;
using ExternalServices;
using ExternalServices.SbQueues;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using ExternalServices.ESignatures.V1;
using CIS.Core;

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
        builder.AddEntityFramework<Database.DocumentOnSAServiceDbContext>(connectionStringKey: CisGlobalConstants.DefaultConnectionStringKey);

        builder.Services.AddHouseholdService()
                    .AddSalesArrangementService()
                    .AddCodebookService()
                    .AddDataAggregatorService()
                    .AddDocumentArchiveService()
                    .AddProductService()
                    .AddCaseService()
                    .AddCustomerService()
                    .AddUserService()
                    .AddDocumentGeneratorService();

        // EAS svc
        builder.AddExternalService<IEasClient>();

        // sulm
        builder.AddExternalService<ISulmClient>();

        // ePodpisy
        builder.AddExternalService<IESignaturesClient>();
        
        // ePodpisy fronta
        builder.AddExternalService<ISbQueuesRepository>();
        
        // registrace background jobu
        builder.AddCisBackgroundService<CheckDocumentsArchivedJob>();
        builder.AddCisBackgroundService<CheckDocumentsArchivedJob, CheckDocumentsArchivedJobConfiguration>();
        builder.AddCisBackgroundService<UpdateDocumentStatusJob>();

        return builder;
    }
}
