using CIS.ExternalServicesHelpers;
using CIS.Foms.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace ExternalServices.Sulm;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddExternalServiceSulm(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfigurationWithServiceDiscovery<SulmConfiguration>("Sulm");

        switch (configuration.Version, configuration.ImplementationType)
        {
            case (Versions.V1, ServiceImplementationTypes.Mock):
                builder.Services.AddScoped<V1.ISulmClient, V1.MockSulmClient>();
                break;

            case (Versions.V1, ServiceImplementationTypes.Real):
                builder.Services
                    .AddHttpClient<V1.ISulmClient, V1.RealSulmClient>((services, client) =>
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
                throw new NotImplementedException($"SULM version {configuration.Version} client not implemented");
        }

        return builder;
    }
}
