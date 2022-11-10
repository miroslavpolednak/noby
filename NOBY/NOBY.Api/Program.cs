using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Abstraction;
using DomainServices.ProductService.Clients;
using DomainServices.CaseService.Clients;
using DomainServices.UserService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.RiskIntegrationService.Clients;
using NOBY.Api.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using DomainServices.HouseholdService.Clients;
using CIS.Infrastructure.MediatR;

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
    .AddHouseholdService()
    .AddOfferService()
    .AddRiskIntegrationService()
    .AddCodebookService()
    .AddCustomerService()
    .AddProductService()
    .AddUserService()
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