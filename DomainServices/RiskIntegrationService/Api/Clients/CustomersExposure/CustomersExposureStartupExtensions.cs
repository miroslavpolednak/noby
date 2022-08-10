using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class CustomersExposureStartupExtensions
{
    internal const string ServiceName = "C4MCustomersExposure";

    public static WebApplicationBuilder AddCustomersExposure(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<CustomersExposure.Configuration.CustomersExposureConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version)
            {
                case Versions.V1:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<CustomersExposure.V1.ICustomersExposureClient, CustomersExposure.V1.MockCustomersExposureClient>();
                    else
                        builder.Services
                            .AddC4mHttpClient<CustomersExposure.V1.ICustomersExposureClient, CustomersExposure.V1.RealCustomersExposureClient>(configuration)
                            .ConfigureC4mHttpMessageHandler<CustomersExposure.V1.RealCustomersExposureClient>(ServiceName)
                            .AddC4mPolicyHandler<CustomersExposure.V1.ICustomersExposureClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}
