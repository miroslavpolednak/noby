using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Configuration;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile;

internal static class CustomerProfileStartupExtensions
{
    public static IServiceCollection AddCustomerProfileService(this IServiceCollection services, CustomerManagementConfiguration config)
    {
        switch (config.Version, config.ImplementationType)
        {
            case (CMVersion.V1, ServiceImplementationTypes.Real):
                services.AddHttpClient<V1.ICustomerProfileClient, V1.RealCustomerProfileClient>((provider, client) =>
                {
                    client.BaseAddress = GetClientBaseAddress(provider);
                    client.DefaultRequestHeaders.Authorization = config.HttpBasicAuth;
                }).ConfigurePrimaryHttpMessageHandler<CertificationValidatorHttpHandler>();
                break;

            case (CMVersion.V1, _):
                services.AddScoped<V1.ICustomerProfileClient, V1.MockCustomerProfileClient>();
                break;

            default:
                throw new NotImplementedException($"CustomerProfile version {config.Version} client is not implemented");
        }

        return services;
    }

    private static Uri GetClientBaseAddress(IServiceProvider serviceProvider) => new(serviceProvider.GetRequiredService<CustomerManagementConfiguration>().ServiceUrl);
}