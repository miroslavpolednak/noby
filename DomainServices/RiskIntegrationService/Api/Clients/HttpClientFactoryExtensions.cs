using CIS.ExternalServicesHelpers.Configuration;
using System.Diagnostics;
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

    public static IHttpClientBuilder AddC4mHttpClient<TClient, TImplementation, TConfiguration>(this IServiceCollection services, string version)
        where TClient : class
        where TImplementation : class, TClient
        where TConfiguration : class, IExternalServiceBasicAuthenticationConfiguration
        => services.AddHttpClient<TClient, TImplementation>((services, client) =>
        {
            var configuration = services
                .GetService<List<TConfiguration>>()
                ?.FirstOrDefault(t => t.GetVersion() == version)
                ?? throw new CisConfigurationNotFound($"External service configuration of type {typeof(TConfiguration)} for {typeof(TClient)} version {version} not found");

            // service url
            client.BaseAddress = new Uri(configuration.ServiceUrl);

            // auth
            client.DefaultRequestHeaders.Authorization = configuration.HttpBasicAuthenticationHeader;

            // kb hlavicky
            var userInstance = services.GetRequiredService<CIS.Core.Security.ICurrentUserAccessor>();
            
            client.DefaultRequestHeaders.Add("X-KB-Caller-System-Identity", "{\"app\":\"NOBY\",\"appComp\":\"NOBY\"}");
            if (Activity.Current?.Id is not null)
            {
                client.DefaultRequestHeaders.Add("X-B3-TraceId", Activity.Current?.RootId);
                client.DefaultRequestHeaders.Add("X-B3-SpanId", Activity.Current?.SpanId.ToString());
            }
            if (userInstance.IsAuthenticated)
                client.DefaultRequestHeaders.Add("X-KB-Party-Identity-In-Service", "{\"partyIdIS\":[{\"partyId\":{\"id\":\"" + userInstance.User?.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\",\"idScheme\":\"V33\"},\"usg\":\"AUTH\"}]}");
        });
}
