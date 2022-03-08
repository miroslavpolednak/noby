using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.UserService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using FOMS.Api.StartupExtensions;
using CIS.Infrastructure.Telemetry;

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
    .AddCisCurrentUser()
    .AddCisWebApiCors()
    .AddCisLogging()
    .AddCisTracing()
    .AddCisHealthChecks();

// add domain services
builder.Services
    .AddOfferService(true)
    .AddCodebookService(true)
    .AddCustomerService(true)
    .AddProductService(true)
    .AddUserService(true)
    .AddCaseService(true)
    .AddSalesArrangementService(true);

// FOMS services
builder
    .AddFomsServices(appConfiguration)
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

app.Run();