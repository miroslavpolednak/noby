﻿using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MLoanApplicationAssessment";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.ILoanApplicationAssessmentClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.ILoanApplicationAssessmentClient, V1.MockLoanApplicationAssessmentClient>();
                break;

            case (V1.ILoanApplicationAssessmentClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<V1.ILoanApplicationAssessmentClient, V1.RealLoanApplicationAssessmentClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName)
                    .AddBadRequestHandling();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ILoanApplicationAssessmentClient)) => V1.ILoanApplicationAssessmentClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
