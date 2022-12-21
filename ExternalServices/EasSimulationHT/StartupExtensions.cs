using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "EasSimulationHT";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, EasSimulationHT.V6.IEasSimulationHTClient
        => builder.AddEasSimulationHT<TClient>(EasSimulationHT.V6.IEasSimulationHTClient.Version);

    private static WebApplicationBuilder AddEasSimulationHT<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (EasSimulationHT.V6.IEasSimulationHTClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddScoped<EasSimulationHT.V6.IEasSimulationHTClient, EasSimulationHT.V6.MockEasSimulationHTClient>();
                break;

            case (EasSimulationHT.V6.IEasSimulationHTClient.Version, ServiceImplementationTypes.Real):
                    builder.Services.AddScoped<EasSimulationHT.V6.IEasSimulationHTClient, EasSimulationHT.V6.RealEasSimulationHTClient>();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
