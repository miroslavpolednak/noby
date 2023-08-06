using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "CustomerAddressService";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder) where TClient : class, Address.V2.ICustomerAddressServiceClient =>
        builder.AddCustomerAddress<TClient>(Address.V2.ICustomerAddressServiceClient.Version);

    private static WebApplicationBuilder AddCustomerAddress<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (Address.V2.ICustomerAddressServiceClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<Address.V2.ICustomerAddressServiceClient, Address.V2.MockCustomerAddressServiceClient>();
                break;

            case (Address.V2.ICustomerAddressServiceClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<Address.V2.ICustomerAddressServiceClient, Address.V2.RealCustomerAddressServiceClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}