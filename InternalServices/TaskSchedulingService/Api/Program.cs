using CIS.Infrastructure.WebApi;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using DomainServices;
using CIS.InternalServices;
using CIS.InternalServices.TaskSchedulingService.Api;
using CIS.InternalServices.TaskSchedulingService.Api.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

var log = builder.CreateStartupLogger();
try
{
    #region register services
    // konfigurace aplikace
    var envConfiguration = builder.AddCisEnvironmentConfiguration();

    // vlozit do DI vsechny custom services
    builder.Services.AddAttributedServices(typeof(IApiAssembly));

    builder
        .AddCisCoreFeatures(true, true)
        .AddCisLogging()
        .AddCisHealthChecks();

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
        .AddDocumentArchiveService()
        // add internal services
        .AddDataAggregatorService()
        .AddDocumentGeneratorService();

    #endregion register services

    // BUILD APP
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();

    app
        // dashboard pro scheduler
        .AddSchedulerUI()
        // health check call - neni treba poustet celou pipeline
        .UseServiceHealthChecks();

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