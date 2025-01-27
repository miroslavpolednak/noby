﻿using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.OfferService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddOfferService(this WebApplicationBuilder builder)
    {
        // EAS EasSimulationHT svc
        builder.AddExternalService<ExternalServices.EasSimulationHT.V1.IEasSimulationHTClient>();

        builder.Services.AddRiskIntegrationService();

        // dbcontext
        builder.AddEntityFramework<Database.OfferServiceDbContext>();

        return builder;
    }
}
