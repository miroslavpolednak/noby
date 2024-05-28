﻿using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using SharedTypes.Enums;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "CustomerManagement";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder) where TClient : class, CustomerManagement.V2.ICustomerManagementClient
        => builder.AddCustomerManagement<TClient>(CustomerManagement.V2.ICustomerManagementClient.Version);

    private static WebApplicationBuilder AddCustomerManagement<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (CustomerManagement.V2.ICustomerManagementClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<CustomerManagement.V2.ICustomerManagementClient, CustomerManagement.V2.MockCustomerManagementClient>();
                break;

            case (CustomerManagement.V2.ICustomerManagementClient.Version, ServiceImplementationTypes.Real):
                AddRestClient<CustomerManagement.V2.ICustomerManagementClient, CustomerManagement.V2.RealCustomerManagementClient>(builder, configuration.Username!);
                break;
            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    private static void AddRestClient<TClient, TImplementation>(WebApplicationBuilder builder, in string fallbackUsername)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
    {
        builder.AddExternalServiceRestClient<TClient, TImplementation>()
               .AddExternalServicesKbHeaders()
			   .AddExternalServicesKbPartyHeadersWithFallback(new SharedTypes.Types.UserIdentity(fallbackUsername, UserIdentitySchemes.KbUms))
			   .AddExternalServicesErrorHandling(ServiceName);
    }
}
