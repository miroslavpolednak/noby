﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using CIS.ExternalServicesHelpers;
using CIS.InternalServices.ServiceDiscovery.Clients;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalServiceSbWebApi(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<Configuration.SbWebApiConfiguration>(ServiceName);

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.ISbWebApiClient, V1.MockSbWebApiClient>();
                else
                    builder.Services.AddHttpClient<V1.ISbWebApiClient, V1.RealSbWebApiClient>((services, client) =>
                    {
                        // service url
                        if (configuration.UseServiceDiscovery)
                        {
                            string url = services
                                .GetRequiredService<IDiscoveryServiceAbstraction>()
                                .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{ServiceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                            client.BaseAddress = new Uri(url!);
                        }
                        else
                            client.BaseAddress = new Uri(configuration.ServiceUrl);
                    });
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
        }

        return builder;
    }

    
}
