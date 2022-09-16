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
            switch (configuration.Version)
            {
                case Versions.V0_2:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<RiskBusinessCase.V0_2.IRiskBusinessCaseClient, RiskBusinessCase.V0_2.MockRiskBusinessCaseClient>();
                    else
                        builder.Services
                            .AddC4mHttpClient<RiskBusinessCase.V0_2.IRiskBusinessCaseClient, RiskBusinessCase.V0_2.RealRiskBusinessCaseClient, RiskBusinessCaseConfiguration>(Versions.V0_2.ToString())
                            .ConfigureC4mHttpMessageHandler<RiskBusinessCase.V0_2.RealRiskBusinessCaseClient>(ServiceName);
                    break;

                case Versions.V1:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<RiskBusinessCase.V1.IRiskBusinessCaseClient, RiskBusinessCase.V1.MockRiskBusinessCaseClient>();
                    else
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