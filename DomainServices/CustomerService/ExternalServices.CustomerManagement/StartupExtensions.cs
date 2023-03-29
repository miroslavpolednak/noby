using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "CustomerManagement";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, CustomerManagement.V1.ICustomerManagementClient
        => builder.AddCustomerManagement<TClient>(CustomerManagement.V1.ICustomerManagementClient.Version);

    static WebApplicationBuilder AddCustomerManagement<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (CustomerManagement.V1.ICustomerManagementClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<CustomerManagement.V1.ICustomerManagementClient, CustomerManagement.V1.MockCustomerManagementClient>();
                break;

            case (CustomerManagement.V1.ICustomerManagementClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<CustomerManagement.V1.ICustomerManagementClient, CustomerManagement.V1.RealCustomerManagementClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
