using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CodebookService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "RDM";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, RDM.V1.IRDMClient
        => builder.AddAcvEnumService<TClient>(RDM.V1.IRDMClient.Version);

    static WebApplicationBuilder AddAcvEnumService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (RDM.V1.IRDMClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<RDM.V1.IRDMClient, RDM.V1.MockRDMClient>();
                break;

            case (RDM.V1.IRDMClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<RDM.V1.IRDMClient, RDM.V1.RealRDMClient>()
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
