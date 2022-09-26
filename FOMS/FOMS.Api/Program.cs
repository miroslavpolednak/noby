using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.UserService.Clients;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.RiskIntegrationService.Clients;
using FOMS.Api.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using DomainServices.RiskIntegrationService.Clients;

var builder = WebApplication.CreateBuilder(args);

#region register services
// konfigurace aplikace
var appConfiguration = builder.AddFomsConfig();

// vlozit do DI vsechny custom services
builder.Services.AddAttributedServices(typeof(FOMS.Infrastructure.IInfrastructureAssembly), typeof(FOMS.Api.IApiAssembly), typeof(FOMS.Services.IServicesAssembly));

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
    .AddFomsServices()
    .AddFomsDatabase();
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