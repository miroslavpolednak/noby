﻿using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient, ExternalServiceConfiguration<TClient>>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.ISbWebApiClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.ISbWebApiClient, V1.MockSbWebApiClient>();
                break;

            case (V1.ISbWebApiClient.Version, ServiceImplementationTypes.Real):
                builder.AddExternalServiceRestClient<V1.ISbWebApiClient, V1.RealSbWebApiClient, ExternalServiceConfiguration<V1.ISbWebApiClient>>(V1.ISbWebApiClient.Version, configuration, _addAdditionalHttpHandlers);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ISbWebApiClient)) => V1.ISbWebApiClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static Action<IHttpClientBuilder, IExternalServiceConfiguration> _addAdditionalHttpHandlers = (builder, configuration)
        => builder
            .AddExternalServicesCorrelationIdForwarding()
            .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
}
