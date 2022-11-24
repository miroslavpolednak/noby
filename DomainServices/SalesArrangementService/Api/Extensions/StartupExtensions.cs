using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Eas;
using ExternalServices.SbWebApi;
using ExternalServices.Sulm;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.EAS is null)
            throw new CisConfigurationNotFound("AppConfiguration");
    }

    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);
        // sulm
        builder.AddExternalServiceSulm();
        // sb web api
        builder.AddExternalServiceSbWebApi();

        // dbcontext
        builder.AddEntityFramework<Repositories.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Repositories.NobyDbContext>(connectionStringKey: "nobyDb");

        return builder;
    }
}
