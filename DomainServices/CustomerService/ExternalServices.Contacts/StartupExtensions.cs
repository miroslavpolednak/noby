using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.Contacts.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Contacts";
    
    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder) where TClient : class, IContactClient
        => builder.AddContactService<TClient>(IContactClient.Version);

    private static WebApplicationBuilder AddContactService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (IContactClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<IContactClient, MockContactClient>();
                break;

            case (IContactClient.Version, ServiceImplementationTypes.Real):
                AddRestClient<IContactClient, RealContactClient>(builder);
                break;
            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    private static void AddRestClient<TClient, TImplementation>(WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
    {
        builder.AddExternalServiceRestClient<TClient, TImplementation>()
               .AddExternalServicesKbHeaders()
               .AddExternalServicesKbPartyHeaders()
               .AddExternalServicesErrorHandling(ServiceName);
    }
}