using CIS.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Sulm;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddExternalServiceSulm(this WebApplicationBuilder builder)
    {
        var sulmConfiguration = builder.CreateAndCheckExternalServiceConfigurationWithServiceDiscovery<SulmConfiguration>("Sulm");

        switch (sulmConfiguration.Version)
        {
            case Versions.V1:
                if (sulmConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.ISulmClient, V1.MockSulmClient>();
                else
                    builder.Services.AddScoped<V1.ISulmClient, V1.RealSulmClient>();
                break;

            default:
                throw new NotImplementedException($"SULM version {sulmConfiguration.Version} client not implemented");
        }

        return builder;
    }
}
