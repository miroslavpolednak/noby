using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Sulm";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, Sulm.V1.ISulmClient
        => builder.AddSulm<TClient>(Sulm.V1.ISulmClient.Version);

    private static WebApplicationBuilder AddSulm<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (Sulm.V1.ISulmClient.Version, ServiceImplementationTypes.Mock):
                builder.Services
                    .AddScoped<Sulm.V1.ISulmClientHelper, Sulm.V1.SulmClientHelper>()
                    .AddScoped<Sulm.V1.ISulmClient, Sulm.V1.MockSulmClient>();
                break;

            case (Sulm.V1.ISulmClient.Version, ServiceImplementationTypes.Real):
                builder.Services.AddScoped<Sulm.V1.ISulmClientHelper, Sulm.V1.SulmClientHelper>();
                builder
                    .AddExternalServiceRestClient<Sulm.V1.ISulmClient, Sulm.V1.RealSulmClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
