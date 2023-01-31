using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Kyc";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, Kyc.V1.IKycClient
        => builder.AddKyc<TClient>(Kyc.V1.IKycClient.Version);

    static WebApplicationBuilder AddKyc<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (Kyc.V1.IKycClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<Kyc.V1.IKycClient, Kyc.V1.MockKycClient>();
                break;

            case (Kyc.V1.IKycClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<Kyc.V1.IKycClient, Kyc.V1.RealKycClient>()
                    .AddExternalServicesKbHeaders("CUSTOMER_SERVICE")
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
