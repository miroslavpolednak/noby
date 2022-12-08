using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Foms.Enums;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "MpHome";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (MpHome.V1_1.IMpHomeClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<MpHome.V1_1.IMpHomeClient, MpHome.V1_1.MockMpHomeClient>();
                break;

            case (MpHome.V1_1.IMpHomeClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<MpHome.V1_1.IMpHomeClient, MpHome.V1_1.RealMpHomeClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(MpHome.V1_1.IMpHomeClient)) => MpHome.V1_1.IMpHomeClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
