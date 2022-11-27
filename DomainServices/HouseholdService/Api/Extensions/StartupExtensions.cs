using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Eas;
using ExternalServices.Sulm;

namespace DomainServices.HouseholdService.Api;

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

    public static WebApplicationBuilder AddHouseholdService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);
        // sulm
        builder.AddExternalServiceSulm();

        // dbcontext
        builder.AddEntityFramework<Database.HouseholdServiceDbContext>();

        return builder;
    }
}
