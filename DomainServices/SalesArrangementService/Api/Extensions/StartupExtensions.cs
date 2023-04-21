using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        // dbcontext
        builder.AddEntityFramework<Database.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Database.DocumentArchiveService.DocumentArchiveServiceDbContext>(connectionStringKey: "documentArchiveDb");

        // background svc
        builder.AddCisBackgroundService<BackgroundServices.OfferGuaranteeDateToCheck.OfferGuaranteeDateToCheckJob>();

        return builder;
    }
}
