using CIS.Core.Types;
using CIS.ExternalServicesHelpers;
using CIS.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using CIS.InternalServices.ServiceDiscovery.Contracts;
using DomainServices.CustomerService.Api.Clients.CustomerManagement;
using DomainServices.CustomerService.Api.Clients.CustomerProfile;
using DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr;
using DomainServices.CustomerService.Api.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CustomerService.Api.Clients;

internal static class ClientsStartupExtensions
{
    private const string CustomerManagementServiceName = "CustomerManagement";

    public static WebApplicationBuilder AddCustomerManagementService(this WebApplicationBuilder builder)
    {
        var config = builder.CreateAndCheckExternalServiceConfiguration<CustomerManagementConfiguration>(CustomerManagementServiceName);

        builder.Services
               .AddCustomerManagementService(config)
               .AddCustomerProfileService(config)
               .AddIdentifiedSubjectService(config)
               .ResolveServiceDiscoveryUriIfEnabled(config);

        return builder;
    }

    private static void ResolveServiceDiscoveryUriIfEnabled<TConfig>(this IServiceCollection services, TConfig config) 
        where TConfig : class, IExternalServiceConfiguration
    {
        if (!config.UseServiceDiscovery)
            return;

        services.Replace(ServiceDescriptor.Singleton(DiscoverAndSetServiceUrl));

        TConfig DiscoverAndSetServiceUrl(IServiceProvider serviceProvider)
        {
            var name = new ServiceName(Constants.ExternalServicesServiceDiscoveryKeyPrefix + CustomerManagementServiceName);

            config.ServiceUrl = serviceProvider.GetRequiredService<IDiscoveryServiceAbstraction>()
                                               .GetServiceUrlSynchronously(name, ServiceTypes.Proprietary);

            return config;
        }
    }
}