using CIS.Core.Types;
using CIS.ExternalServicesHelpers;
using CIS.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using CIS.InternalServices.ServiceDiscovery.Contracts;
using DomainServices.CustomerService.Api.Clients.CustomerProfile;
using DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr;
using DomainServices.CustomerService.Api.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CustomerService.Api.Clients;

internal static class ClientsStartupExtensions
{
    private const string CustomerManagementServiceName = "CustomerManagement";
    private const string IdentifiedSubjectBrServiceName = "IdentifiedSubjectBr";

    public static WebApplicationBuilder AddCustomerManagementService(this WebApplicationBuilder builder)
    {
        AddCustomerManagementServices(builder);
        AddIdentifiedSubjectBrService(builder);

        return builder;
    }

    private static void AddCustomerManagementServices(WebApplicationBuilder builder)
    {
        var config = builder.CreateAndCheckExternalServiceConfiguration<CustomerManagementConfiguration>(CustomerManagementServiceName);

        builder.Services
               .AddCustomerProfileService(config)
               .ResolveServiceDiscoveryUriIfEnabled(config, CustomerManagementServiceName);
    }

    private static void AddIdentifiedSubjectBrService(WebApplicationBuilder builder)
    {
        var config = builder.CreateAndCheckExternalServiceConfiguration<IdentifiedSubjectBrConfiguration>(IdentifiedSubjectBrServiceName);

        builder.Services.AddIdentifiedSubjectService(config).ResolveServiceDiscoveryUriIfEnabled(config, IdentifiedSubjectBrServiceName);
    }

    private static void ResolveServiceDiscoveryUriIfEnabled<TConfig>(this IServiceCollection services, TConfig config, string serviceName) 
        where TConfig : class, IExternalServiceConfiguration
    {
        if (!config.UseServiceDiscovery)
            return;

        services.Replace(ServiceDescriptor.Singleton(DiscoverAndSetServiceUrl));

        TConfig DiscoverAndSetServiceUrl(IServiceProvider serviceProvider)
        {
            var name = new ApplicationKey(Constants.ExternalServicesServiceDiscoveryKeyPrefix + serviceName);

            config.ServiceUrl = serviceProvider.GetRequiredService<IDiscoveryServiceClient>()
                                               .GetServiceUrlSynchronously(name, ServiceTypes.Proprietary);

            return config;
        }
    }
}