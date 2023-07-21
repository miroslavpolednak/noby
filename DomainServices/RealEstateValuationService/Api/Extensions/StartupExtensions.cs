using CIS.Infrastructure.StartupExtensions;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.ExternalServices;

namespace DomainServices.RealEstateValuationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRealEstateValuationService(this WebApplicationBuilder builder)
    {
        // dbcontext
        builder.AddEntityFramework<RealEstateValuationServiceDbContext>();

        builder.AddExternalService<ExternalServices.PreorderService.V1.IPreorderServiceClient>();

        return builder;
    }
}
