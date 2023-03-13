using CIS.Infrastructure.StartupExtensions;
using ExternalServices;
using ExternalServices.SbWebApi.V1;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();
        // sb web api
        builder.AddExternalService<ISbWebApiClient>();

        // dbcontext
        builder.AddEntityFramework<Database.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Database.NobyDbContext>(connectionStringKey: "nobyDb");

        return builder;
    }
}
