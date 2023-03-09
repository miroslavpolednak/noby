using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "CustomerProfile";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, CustomerProfile.V1.ICustomerProfileClient
        => builder.AddCustomerProfile<TClient>(CustomerProfile.V1.ICustomerProfileClient.Version);

    static WebApplicationBuilder AddCustomerProfile<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (CustomerProfile.V1.ICustomerProfileClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<CustomerProfile.V1.ICustomerProfileClient, CustomerProfile.V1.MockCustomerProfileClient>();
                break;

            case (CustomerProfile.V1.ICustomerProfileClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<CustomerProfile.V1.ICustomerProfileClient, CustomerProfile.V1.RealCustomerProfileClient>()
<<<<<<< HEAD
                    .AddExternalServicesKbHeaders()
=======
                    .AddExternalServicesKbHeaders("CUSTOMER_SERVICE")
                    .AddExternalServicesKbPartyHeaders()
>>>>>>> feature/HFICH-2900
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
