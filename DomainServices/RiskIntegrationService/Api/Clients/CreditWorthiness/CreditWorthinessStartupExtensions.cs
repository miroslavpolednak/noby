using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class CreditWorthinessStartupExtensions
{
    internal const string ServiceName = "C4MCreditWorthiness";

    public static WebApplicationBuilder AddCreditWorthiness(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<CreditWorthiness.Configuration.CreditWorthinessConfiguration>(ServiceName);

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.MockCreditWorthinessClient> ();
                else
                    builder.Services
                        .AddHttpClient< CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.RealCreditWorthinessClient> ((services, client) =>
                        {
                        // service url
                            client.BaseAddress = new Uri(configuration.ServiceUrl);

                        // auth
                            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                        })
                        .AddPolicyHandler((services, request) => HttpPolicyExtensions
                            .HandleTransientHttpError()
                            .WaitAndRetryAsync(new[]
                            {
                            TimeSpan.FromSeconds(1)
                            },
                            onRetry: (outcome, timespan, retryAttempt, context) =>
                            {
                                services.GetService<ILogger<CreditWorthiness.V1.ICreditWorthinessClient>>()?.ExtServiceRetryCall(ServiceName, retryAttempt, timespan.TotalMilliseconds);
                            }
                            ));
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
        }

        return builder;
    }
}
