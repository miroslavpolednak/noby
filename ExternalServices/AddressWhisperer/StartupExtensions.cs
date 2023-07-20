using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "AddressWhisperer";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, AddressWhisperer.V1.IAddressWhispererClient
        => builder.AddAddressWhisperer<TClient>(AddressWhisperer.V1.IAddressWhispererClient.Version);

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder, string kbHeaderAppComponent, string kbHeaderAppComponentOriginator)
        where TClient : class, AddressWhisperer.V1.IAddressWhispererClient
        => builder.AddAddressWhisperer<TClient>(AddressWhisperer.V1.IAddressWhispererClient.Version, kbHeaderAppComponent, kbHeaderAppComponentOriginator);

    private static WebApplicationBuilder AddAddressWhisperer<TClient>(this WebApplicationBuilder builder, string version, string? kbHeaderAppComponent = null, string? kbHeaderAppComponentOriginator = null)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (AddressWhisperer.V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<AddressWhisperer.V1.IAddressWhispererClient, AddressWhisperer.V1.MockAddressWhispererClient>();
                break;

            case (AddressWhisperer.V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<AddressWhisperer.V1.IAddressWhispererClient, AddressWhisperer.V1.RealAddressWhispererClient>()
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
