﻿using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentOnSAService.Api.Configuration;
using ExternalServices;

namespace DomainServices.DocumentOnSAService.Api.Extensions;

internal static class StartupExtensions
{
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
    }

    public static WebApplicationBuilder AddDocumentOnSAServiceService(this WebApplicationBuilder builder)
    {
        // dbcontext
        builder.AddEntityFramework<Database.DocumentOnSAServiceDbContext>(connectionStringKey: "default");

        builder.Services.AddHouseholdService();

        builder.Services.AddSalesArrangementService();

        builder.Services.AddCaseService();

        builder.Services.AddCodebookService();

        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.R21.IEasClient>();

        return builder;
    }

}
