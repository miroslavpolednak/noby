﻿using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Eas;
using ExternalServices.EasSimulationHT;
using ExternalServices.SbWebApi;
using ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.EAS == null)
            throw new CisConfigurationNotFound("EAS");
    }

    public static WebApplicationBuilder AddCaseService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);

        // EAS EasSimulationHT svc
        builder.Services.AddExternalServiceEasSimulationHT(appConfiguration.EasSimulationHT);

        // SB webapi svc
        builder.AddExternalService<ISbWebApiClient>();

        // dbcontext
        builder.AddEntityFramework<Repositories.CaseServiceDbContext>();

        return builder;
    }
}
