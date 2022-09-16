﻿using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Configuration;

namespace DomainServices.CustomerService.Api.Clients.CustomerManagement;

internal static class CustomerManagementStartupExtensions
{
    public static IServiceCollection AddCustomerManagementService(this IServiceCollection services, CustomerManagementConfiguration config)
    {
        switch (config.CustomerManagementVersion, config.ImplementationType)
        {
            case (Version.V1, ServiceImplementationTypes.Real):
                services.AddHttpClient<V1.ICustomerManagementClient, V1.RealCustomerManagementClient>((provider, client) =>
                {
                    client.BaseAddress = GetClientBaseAddress(provider);
                    client.DefaultRequestHeaders.Authorization = config.HttpBasicAuth;
                }).ConfigurePrimaryHttpMessageHandler<CertificationValidatorHttpHandler>();
                break;

            case (Version.V1, _):
                services.AddScoped<V1.ICustomerManagementClient, V1.MockCustomerManagementClient>();
                break;

            default:
                throw new NotImplementedException($"CustomerManagement version {config.CustomerManagementVersion} client is not implemented");
        }

        return services;
    }

    private static Uri GetClientBaseAddress(IServiceProvider serviceProvider) => new(serviceProvider.GetRequiredService<CustomerManagementConfiguration>().ServiceUrl);
}