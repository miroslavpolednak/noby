﻿using SharedTypes.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MCustomerExposure";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ICustomerExposureClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (CustomerExposure.V3.ICustomerExposureClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<CustomerExposure.V3.ICustomerExposureClient, CustomerExposure.V3.MockCustomerExposureClient>();
                break;

            case (CustomerExposure.V3.ICustomerExposureClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<CustomerExposure.V3.ICustomerExposureClient, CustomerExposure.V3.RealCustomerExposureClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName)
                    .AddBadRequestHandling();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        ErrorCodeMapper.Init();

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(CustomerExposure.V3.ICustomerExposureClient)) => CustomerExposure.V3.ICustomerExposureClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
