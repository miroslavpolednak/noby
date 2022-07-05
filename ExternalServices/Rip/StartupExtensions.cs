using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System.Text;
using Microsoft.AspNetCore.Builder;
using CIS.ExternalServicesHelpers;

namespace ExternalServices.Rip;

public static class StartupExtensions
{
    internal const string ServiceName = "Rip";

    public static WebApplicationBuilder AddExternalServiceRip(this WebApplicationBuilder builder)
    {
        var ripConfiguration = builder.CreateAndCheckExternalServiceConfiguration<RipConfiguration>(ServiceName);

        switch (ripConfiguration.Version)
        {
            case Versions.V1:
                if (ripConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.IRipClient, V1.MockRipClient>();
                else
                    builder.Services.AddHttpClient<V1.IRipClient, V1.RealRipClient>((services, client) =>
                    {
                        var byteArray = Encoding.ASCII.GetBytes($"{ripConfiguration.Username}:{ripConfiguration.Password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        // service url
                        if (ripConfiguration.UseServiceDiscovery)
                        {
                            string url = services
                                .GetRequiredService<IDiscoveryServiceAbstraction>()
                                .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{ServiceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                            client.BaseAddress = new Uri(url!);
                        }
                        else
                            client.BaseAddress = new Uri(ripConfiguration.ServiceUrl);
                    });
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {ripConfiguration.Version} client not implemented");
        }

        return builder;
    }
}
