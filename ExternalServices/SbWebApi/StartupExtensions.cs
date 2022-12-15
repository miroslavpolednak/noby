using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, SbWebApi.V1.ISbWebApiClient
        => builder.AddSbWebApi<TClient>();

    static WebApplicationBuilder AddSbWebApi<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (SbWebApi.V1.ISbWebApiClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<SbWebApi.V1.ISbWebApiClient, SbWebApi.V1.MockSbWebApiClient>();
                break;

            case (SbWebApi.V1.ISbWebApiClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<SbWebApi.V1.ISbWebApiClient, SbWebApi.V1.RealSbWebApiClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(SbWebApi.V1.ISbWebApiClient)) => SbWebApi.V1.ISbWebApiClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
