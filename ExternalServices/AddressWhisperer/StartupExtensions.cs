using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace ExternalServices.AddressWhisperer;

public static class StartupExtensions
{
    internal const string ServiceName = "AddressWhisperer";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, V1.IAddressWhispererClient
        => builder.AddAddressWhisperer<TClient>();

    private static WebApplicationBuilder AddAddressWhisperer<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddScoped<V1.IAddressWhispererClient, V1.MockAddressWhispererClient>();
                break;

            case (V1.IAddressWhispererClient.Version, ServiceImplementationTypes.Real):
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
                throw new NotImplementedException($"AddressWhisperer version {version} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.IAddressWhispererClient)) => V1.IAddressWhispererClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
