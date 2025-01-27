﻿using SharedTypes.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MLoanApplicationAssessment";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ILoanApplicationAssessmentClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V3.MockLoanApplicationAssessmentClient>();
                break;

            case (LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V3.RealLoanApplicationAssessmentClient>()
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
            Type t when t.IsAssignableFrom(typeof(LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient)) => LoanApplicationAssessment.V3.ILoanApplicationAssessmentClient.Version,
            _ => throw new NotImplementedException($"Unknown implementation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
