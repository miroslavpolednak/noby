using CIS.Infrastructure.StartupExtensions;
using DomainServices.RealEstateValuationService.Api.Database;

namespace DomainServices.RealEstateValuationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRealEstateValuationService(this WebApplicationBuilder builder)
    {
        // dbcontext
        builder.AddEntityFramework<RealEstateValuationServiceDbContext>();

        return builder;
    }
}
