using CIS.ExternalServicesHelpers.Configuration;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class HttpClientFactoryExtensions
{
    public static System.Text.Json.JsonSerializerOptions CustomJsonOptions
    {
        get => _jsonOptions;
    }

    private static System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString //TODO odstranit az c4m opravi format cisel
    };

    static HttpClientFactoryExtensions()
    {
        _jsonOptions.Converters.Add(new CIS.Infrastructure.Json.DateTimeOffsetConverterUsingDateTimeParse());
    }

    public static IHttpClientBuilder ConfigureC4mHttpMessageHandler<TClient>(this IHttpClientBuilder builder, string serviceName)
        => builder.ConfigurePrimaryHttpMessageHandler((serviceProvider) => 
        {
            var logger = serviceProvider.GetRequiredService<ILogger<TClient>>();

            return new C4mHttpHandler(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            }, logger, serviceName);
        });

    public static IHttpClientBuilder AddC4mHttpClient<TClient, TImplementation>(this IServiceCollection services, IExternalServiceBasicAuthenticationConfiguration configuration)
        where TClient : class
        where TImplementation : class, TClient
        => services.AddHttpClient<TClient, TImplementation>((services, client) =>
        {
            // service url
            client.BaseAddress = new Uri(configuration.ServiceUrl);

            // auth
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        });
}
