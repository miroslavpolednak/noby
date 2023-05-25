using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Foms.Enums;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "MpHome";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, MpHome.V1.IMpHomeClient
        => builder.AddMpHome<TClient>(MpHome.V1.IMpHomeClient.Version);

    static WebApplicationBuilder AddMpHome<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (MpHome.V1.IMpHomeClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<MpHome.V1.IMpHomeClient, MpHome.V1.MockMpHomeClient>();
                break;

            case (MpHome.V1.IMpHomeClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<MpHome.V1.IMpHomeClient, MpHome.V1.RealMpHomeClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
