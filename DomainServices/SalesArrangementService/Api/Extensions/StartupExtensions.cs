using CIS.Infrastructure.StartupExtensions;
using ExternalServices;
using ExternalServices.SbWebApi.V1;
using ExternalServices.Sulm.V1;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.R21.IEasClient>();
        // sulm
        builder.AddExternalService<ISulmClient>();
        // sb web api
        builder.AddExternalService<ISbWebApiClient>();

        // dbcontext
        builder.AddEntityFramework<Repositories.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Repositories.NobyDbContext>(connectionStringKey: "nobyDb");

        return builder;
    }
}
