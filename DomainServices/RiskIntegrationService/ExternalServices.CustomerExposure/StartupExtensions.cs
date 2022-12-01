using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MCustomersExposure";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient, ExternalServiceConfiguration<TClient>>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.ICustomersExposureClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.ICustomersExposureClient, V1.MockCustomersExposureClient>();
                break;

            case (V1.ICustomersExposureClient.Version, ServiceImplementationTypes.Real):
                builder.AddExternalServiceRestClient<V1.ICustomersExposureClient, V1.RealCustomersExposureClient, ExternalServiceConfiguration<V1.ICustomersExposureClient>>(V1.ICustomersExposureClient.Version, configuration, _addAdditionalHttpHandlers);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ICustomersExposureClient)) => V1.ICustomersExposureClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static Action<IHttpClientBuilder, IExternalServiceConfiguration> _addAdditionalHttpHandlers = (builder, configuration)
        => builder
            .AddExternalServicesKbHeaders()
            .AddExternalServicesErrorHandling(StartupExtensions.ServiceName)
            .AddBadRequestHandling();

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
    {
        builder.Services.AddSingleton(provider => new BadRequestHttpHandler(StartupExtensions.ServiceName));

        return builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
    }
}
