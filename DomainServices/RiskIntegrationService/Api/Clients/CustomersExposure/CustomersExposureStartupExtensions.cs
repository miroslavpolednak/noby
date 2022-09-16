using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure;
using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class CustomersExposureStartupExtensions
{
    internal const string ServiceName = "C4MCustomersExposure";

    public static WebApplicationBuilder AddCustomersExposureClient(this WebApplicationBuilder builder)
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
    }
}
