using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MCustomersExposure";

    /*public static IHttpClientBuilder AddExternalService(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<CustomersExposureConfiguration>(ServiceName);

        configurations.ForEach(configuration =>
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<CustomersExposure.V1.ICustomersExposureClient, CustomersExposure.V1.MockCustomersExposureClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<CustomersExposure.V1.ICustomersExposureClient, CustomersExposure.V1.RealCustomersExposureClient, CustomersExposureConfiguration>(Versions.V1.ToString())
                        .ConfigureC4mHttpMessageHandler<CustomersExposure.V1.RealCustomersExposureClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        });

        return builder;
    }*/

    /*public static IHttpClientBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class
    {
        return typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ICustomersExposureClient))
                => builder
                    .AddExternalServiceRestClient<V1.ICustomersExposureClient, V1.RealCustomersExposureClient, ExternalServiceConfiguration<V1.ICustomersExposureClient>>(ServiceName, "V1", _addAdditionalHttpHandlers),
            _ => throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented")
        };
    }

    private static Action<IHttpClientBuilder, ExternalServiceConfiguration<V1.ICustomersExposureClient>> _addAdditionalHttpHandlers = (builder, configuration)
        => builder
            .AddExternalServicesCorrelationIdForwarding()
            .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);*/
}
