using CIS.Infrastructure.StartupExtensions;
using DomainServices.CaseService.Api.Database;
using ExternalServices;

namespace DomainServices.CaseService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCaseService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();

        // EAS EasSimulationHT svc
        builder.AddExternalService<ExternalServices.EasSimulationHT.V1.IEasSimulationHTClient>();

        // SB webapi svc
        builder.AddExternalService<ExternalServices.SbWebApi.V1.ISbWebApiClient>();

        // dbcontext
        builder.AddEntityFramework<CaseServiceDbContext>();

        return builder;
    }
}
