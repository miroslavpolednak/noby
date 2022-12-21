using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.HouseholdService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddHouseholdService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.R21.IEasClient>();
        // sulm
        builder.AddExternalService<ExternalServices.Sulm.V1.ISulmClient>();

        // dbcontext
        builder.AddEntityFramework<Database.HouseholdServiceDbContext>();

        return builder;
    }
}
