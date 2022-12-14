using CIS.Infrastructure.StartupExtensions;
using NOBY.Api.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.MediatR;
using DomainServices;
using CIS.InternalServices;

var builder = WebApplication.CreateBuilder(args);

#region register services
// konfigurace aplikace
var appConfiguration = builder.AddNobyConfig();

// vlozit do DI vsechny custom services
builder.Services.AddAttributedServices(typeof(NOBY.Infrastructure.IInfrastructureAssembly), typeof(NOBY.Api.IApiAssembly));

// add CIS pipeline
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisWebApiCors()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisHealthChecks();

// add domain services
builder.Services
    .AddUserService()
    .AddHouseholdService()
    .AddOfferService()
    .AddRiskIntegrationService()
    .AddCodebookService()
    .AddCustomerService()
    .AddProductService()
    .AddCaseService()
    .AddSalesArrangementService()
    .AddRiskIntegrationService();

// FOMS services
builder
    .AddNobyServices()
    .AddNobyDatabase();

// authentication
builder.AddFomsAuthentication(appConfiguration);

// swagger
if (appConfiguration.EnableSwaggerUi)
    builder.AddFomsSwagger();

// podpora SPA
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

// pridat moznost rollbacku mediatr handleru
builder.Services.AddCisMediatrRollbackCapability();
#endregion register services

// BUILD APP
var app = builder.Build();

app.UseServiceDiscovery();

app.UseCisWebRequestLocalization();

app
    // API call
    .UseFomsApi()
    // jedna se o SPA call, pust jen tyhle middlewares
    .UseFomsSpa()
    // health check call - neni treba poustet celou pipeline
    .UseFomsHealthChecks();

// swagger
if (appConfiguration.EnableSwaggerUi)
    app.UseFomsSwagger();

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}