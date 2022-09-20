using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Configuration;

namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr;

public static class IdentifiedSubjectStartupExtensions
{
    public static IServiceCollection AddIdentifiedSubjectService(this IServiceCollection services, CustomerManagementConfiguration config)
    {
        switch (config.IdentifiedSubjectVersion, config.ImplementationType)
        {
            case (Version.V1, ServiceImplementationTypes.Real):
                services.AddHttpClient<V1.IIdentifiedSubjectClient, V1.RealIdentifiedSubjectClient>((provider, client) =>
                {
                    client.BaseAddress = GetClientBaseAddress(provider);
                    client.DefaultRequestHeaders.Authorization = config.HttpBasicAuthenticationHeader;
                }).ConfigurePrimaryHttpMessageHandler<CustomerManagementHttpHandler<V1.RealIdentifiedSubjectClient>>();
                break;

            case (Version.V1, _):
                services.AddScoped<V1.IIdentifiedSubjectClient, V1.MockIdentifiedSubjectClient>();
                break;

            default:
                throw new NotImplementedException($"IdentifiedSubject version {config.IdentifiedSubjectVersion} client is not implemented");
        }

        return services;
    }

    private static Uri GetClientBaseAddress(IServiceProvider serviceProvider) => new(serviceProvider.GetRequiredService<CustomerManagementConfiguration>().ServiceUrl);
}