using CIS.ExternalServicesHelpers;
using CIS.Foms.Enums;
using ExternalServices.AddressWhisperer.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.AddressWhisperer;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddExternalServiceAddressWhisperer(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<AddressWhispererConfiguration>("AddressWhisperer");

        switch (configuration.Version, configuration.ImplementationType)
        {
            case (Versions.V1, ServiceImplementationTypes.Mock):
                builder.Services.AddScoped<V1.IAddressWhispererClient, V1.MockAddressWhispererClient>();
                break;

            case (Versions.V1, ServiceImplementationTypes.Real):
                builder.Services.AddScoped<V1.IAddressWhispererClient, V1.RealAddressWhispererClient>();
                break;

            default:
                throw new NotImplementedException($"AddressWhisperer version {configuration.Version} client not implemented");
        }

        return builder;
    }
}
