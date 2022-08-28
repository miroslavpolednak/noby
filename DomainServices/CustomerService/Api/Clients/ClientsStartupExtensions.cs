﻿using CIS.Core.Types;
using CIS.ExternalServicesHelpers;
using CIS.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.InternalServices.ServiceDiscovery.Contracts;
using DomainServices.CustomerService.Api.Clients.CustomerManagement;
using DomainServices.CustomerService.Api.Clients.CustomerProfile;
using DomainServices.CustomerService.Api.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CustomerService.Api.Clients;

public static class ClientsStartupExtensions
{
    private const string CustomerManagementServiceName = "CustomerManagement";
    private const string CustomerProfileServiceName = "CustomerProfile";

    public static WebApplicationBuilder AddCustomerManagementService(this WebApplicationBuilder builder)
    {
        var config = builder.CreateAndCheckExternalServiceConfiguration<CustomerManagementConfiguration>(CustomerManagementServiceName);

        builder.Services.AddCustomerManagementService(config).ResolveServiceDiscoveryUriIfEnabled(config, CustomerManagementServiceName);

        return builder;
    }

    public static WebApplicationBuilder AddCustomerProfileService(this WebApplicationBuilder builder)
    {
        var config = builder.CreateAndCheckExternalServiceConfiguration<CustomerProfileConfiguration>(CustomerProfileServiceName);

        builder.Services.AddCustomerProfileService(config).ResolveServiceDiscoveryUriIfEnabled(config, CustomerProfileServiceName);

        return builder;
    }

    private static void ResolveServiceDiscoveryUriIfEnabled<TConfig>(this IServiceCollection services, TConfig config, string serviceName) where TConfig : class, IExternalServiceConfiguration
    {
        if (!config.UseServiceDiscovery)
            return;

        services.Replace(ServiceDescriptor.Singleton(DiscoverAndSetServiceUrl));

        TConfig DiscoverAndSetServiceUrl(IServiceProvider serviceProvider)
        {
            var name = new ServiceName(Constants.ExternalServicesServiceDiscoveryKeyPrefix + serviceName);

            config.ServiceUrl = serviceProvider.GetRequiredService<IDiscoveryServiceAbstraction>()
                                               .GetServiceUrlSynchronously(name, ServiceTypes.Proprietary);

            return config;
        }
    }
}