﻿using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.OfferService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException("AppConfiguration");
    }

    public static WebApplicationBuilder AddOfferService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS EasSimulationHT svc
        builder.AddExternalService<ExternalServices.EasSimulationHT.V6.IEasSimulationHTClient>();

        // dbcontext
        builder.AddEntityFramework<Database.OfferServiceDbContext>();

        return builder;
    }
}
