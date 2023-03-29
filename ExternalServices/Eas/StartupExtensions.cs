using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "EAS";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, Eas.V1.IEasClient
        => builder.AddEas<TClient>(Eas.V1.IEasClient.Version);

    private static WebApplicationBuilder AddEas<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (Eas.V1.IEasClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddScoped<Eas.V1.IEasClient, Eas.V1.MockEasClient>();
                break;

            case (Eas.V1.IEasClient.Version, ServiceImplementationTypes.Real):
                builder.Services.AddScoped<Eas.V1.IEasClient, Eas.V1.RealEasClient>();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
