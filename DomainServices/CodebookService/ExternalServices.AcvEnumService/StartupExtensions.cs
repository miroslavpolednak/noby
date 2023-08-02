using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CodebookService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "AcvEnumService";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, AcvEnumService.V1.IAcvEnumServiceClient
        => builder.AddAcvEnumService<TClient>(AcvEnumService.V1.IAcvEnumServiceClient.Version);

    static WebApplicationBuilder AddAcvEnumService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (AcvEnumService.V1.IAcvEnumServiceClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<AcvEnumService.V1.IAcvEnumServiceClient, AcvEnumService.V1.MockAcvEnumServiceClient>();
                break;

            case (AcvEnumService.V1.IAcvEnumServiceClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<AcvEnumService.V1.IAcvEnumServiceClient, AcvEnumService.V1.RealAcvEnumServiceClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
