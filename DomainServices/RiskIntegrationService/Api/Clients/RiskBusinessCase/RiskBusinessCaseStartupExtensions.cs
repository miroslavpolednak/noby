using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase;
using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class RiskBusinessCaseStartupExtensions
{
    internal const string ServiceName = "C4MRiskBusinessCase";

    public static WebApplicationBuilder AddRiskBusinessCaseClient(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<RiskBusinessCaseConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<RiskBusinessCase.V1.IRiskBusinessCaseClient, RiskBusinessCase.V1.MockRiskBusinessCaseClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<RiskBusinessCase.V1.IRiskBusinessCaseClient, RiskBusinessCase.V1.RealRiskBusinessCaseClient, RiskBusinessCaseConfiguration>(Versions.V1.ToString())
                        .ConfigureC4mHttpMessageHandler<RiskBusinessCase.V1.RealRiskBusinessCaseClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}