using CIS.ExternalServicesHelpers;
using CIS.Foms.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

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
                builder.Services
                    .AddHttpClient<V1.IAddressWhispererClient, V1.RealAddressWhispererClient>((services, client) =>
                    {
                        // auth
                        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                    })
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                    });
                break;

            default:
                throw new NotImplementedException($"AddressWhisperer version {configuration.Version} client not implemented");
        }

        return builder;
    }
}
