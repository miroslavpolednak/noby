using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Configuration;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile;

internal static class CustomerProfileStartupExtensions
{
    public static IServiceCollection AddCustomerProfileService(this IServiceCollection services, CustomerManagementConfiguration config)
    {
        switch (config.CustomerProfileVersion, config.ImplementationType)
        {
            case (Version.V1, ServiceImplementationTypes.Real):
                services.AddHttpClient<V1.ICustomerProfileClient, V1.RealCustomerProfileClient>((provider, client) =>
                {
                    client.BaseAddress = GetClientBaseAddress(provider);
                    client.DefaultRequestHeaders.Authorization = config.HttpBasicAuthenticationHeader;
                }).ConfigurePrimaryHttpMessageHandler<CustomerManagementHttpHandler<V1.RealCustomerProfileClient>>();
                break;

            default:
                services.AddScoped<V1.ICustomerProfileClient, V1.MockCustomerProfileClient>();
                break;
        }

        return services;
    }

    private static Uri GetClientBaseAddress(IServiceProvider serviceProvider) => new(serviceProvider.GetRequiredService<CustomerManagementConfiguration>().ServiceUrl);
}