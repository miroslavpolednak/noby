using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.AddressWhisperer;

public static class StartupExtensions
{
    internal const string ServiceName = "AddressWhisperer";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, V1.IAddressWhispererClient
        => builder.AddAddressWhisperer<TClient>(V1.IAddressWhispererClient.Version);

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder, string kbHeaderAppComponent, string kbHeaderAppComponentOriginator)
        where TClient : class, V1.IAddressWhispererClient
        => builder.AddAddressWhisperer<TClient>(V1.IAddressWhispererClient.Version, kbHeaderAppComponent, kbHeaderAppComponentOriginator);

    private static WebApplicationBuilder AddAddressWhisperer<TClient>(this WebApplicationBuilder builder, string version, string? kbHeaderAppComponent = null, string? kbHeaderAppComponentOriginator = null)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.IAddressWhispererClient, V1.MockAddressWhispererClient>();
                break;

            case (V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<V1.IAddressWhispererClient, V1.RealAddressWhispererClient>()
                    .AddExternalServicesKbHeaders(kbHeaderAppComponent, kbHeaderAppComponentOriginator)
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
