using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.LoanApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MLoanApplication";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ILoanApplicationClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (LoanApplication.V3.ILoanApplicationClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<LoanApplication.V3.ILoanApplicationClient, LoanApplication.V3.MockLoanApplicationClient>();
                break;

            case (LoanApplication.V3.ILoanApplicationClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<LoanApplication.V3.ILoanApplicationClient, LoanApplication.V3.RealLoanApplicationClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName)
                    .AddBadRequestHandling();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        ErrorCodeMapper.Init();

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(LoanApplication.V3.ILoanApplicationClient)) => LoanApplication.V3.ILoanApplicationClient.Version,
            _ => throw new NotImplementedException($"Unknown implementation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
