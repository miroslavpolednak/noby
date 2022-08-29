using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Configuration;

namespace DomainServices.CustomerService.Api.Clients.CustomerManagement;

public static class CustomerManagementStartupExtensions
{
    public static IServiceCollection AddCustomerManagementService(this IServiceCollection services, CustomerManagementConfiguration config)
    {
        switch (config.Version, config.ImplementationType)
        {
            case (Versions.V1, ServiceImplementationTypes.Real):
                services.AddHttpClient<V1.ICustomerManagementClient, V1.RealCustomerManagementClient>((provider, client) =>
                {
                    client.BaseAddress = GetClientBaseAddress(provider);
                });
                break;

            case (Versions.V1, _):
                services.AddScoped<V1.ICustomerManagementClient, V1.MockCustomerManagementClient>();
                break;

            default:
                throw new NotImplementedException($"CustomerManagement version {config.Version} client is not implemented");
        }

        return services;
    }

    private static Uri GetClientBaseAddress(IServiceProvider serviceProvider) => new(serviceProvider.GetRequiredService<CustomerManagementConfiguration>().ServiceUrl);
}