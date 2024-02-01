using CIS.Infrastructure.StartupExtensions;
using NOBY.Api.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.CisMediatR;
using DomainServices;
using CIS.InternalServices;
using Microsoft.AspNetCore.HttpLogging;
using CIS.Infrastructure.WebApi;
using NOBY.Infrastructure.Configuration;
using SharedAudit;
using SharedComponents.Storage;
using NOBY.Api.Endpoints.Test;

var builder = WebApplication.CreateBuilder(args);

var log = builder.CreateStartupLogger();

try
{
    #region register services
    // konfigurace aplikace
    var envConfiguration = builder.AddCisEnvironmentConfiguration();
    var appConfiguration = builder.AddNobyConfig();

    // vlozit do DI vsechny custom services
    builder.Services.AddAttributedServices(typeof(NOBY.Services.IServicesAssembly), typeof(NOBY.Api.IApiAssembly));

    builder
        .AddCisCoreFeatures(true, true)
        .AddCisWebApiCors()
        .AddCisLogging()
        .AddCisAudit()
        .AddCisTracing()
        .AddCisApiVersioning()
        .AddCisHealthChecks();

    // add temp storage
    builder
        .AddCisStorageServices()
        .AddTempStorage()
        .AddStorageClient<IStorage1>();

    builder.Services.AddCisSecurityHeaders();

    // add .NET logging
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
    });

    // add domain services
    builder.Services
        .AddUserService()
        .AddHouseholdService()
        .AddOfferService()
        .AddRiskIntegrationService()
        .AddCodebookService()
        .AddRealEstateValuationService()
        .AddCustomerService()
        .AddProductService()
        .AddCaseService()
        .AddDocumentOnSAService()
        .AddSalesArrangementService()
        .AddRiskIntegrationService()
        .AddDocumentArchiveService();

    // add internal services
    builder.Services
        .AddDataAggregatorService()
        .AddDocumentGeneratorService();

    // NOBY services
    builder.AddNobyServices(appConfiguration);

    // init validacnich zprav
    ErrorCodeMapper.Init();

    // authentication
    builder.AddNobyAuthentication(appConfiguration);

    // swagger
    if (!envConfiguration.DisableContractDescriptionPropagation)
    {
        builder.AddFomsSwagger();
    }

    // podpora SPA
    builder.Services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "wwwroot";
    });

    // pridat moznost rollbacku mediatr handleru
    builder.Services.AddCisMediatrRollbackCapability(typeof(NOBY.Api.IApiAssembly));
    #endregion register services

    // BUILD APP
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();

    app.UseCisWebRequestLocalization();

    app
        // API call
        .UseNobyApi(appConfiguration)
        // include authentication endpoints
        .UseNobyAuthStrategy()
        // redirect from ZOOM
        .UseRedirectStrategy()
        // jedna se o SPA call, pust jen tyhle middlewares
        .UseNobySpa()
        // health check call - neni treba poustet celou pipeline
        .UseNobyHealthChecks();

    // swagger
    if (!envConfiguration.DisableContractDescriptionPropagation)
    {
        var descriptions = app.DescribeApiVersions();
        app.UseNobySwagger(descriptions);
    }
    
    log.ApplicationRun();
    app.Run();
}
catch (Exception ex)
{
    log.CatchedException(ex);
}
finally
{
    LoggingExtensions.CloseAndFlush();
}