using CIS.Infrastructure.StartupExtensions;
using DomainServices.OfferService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.UserService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using FOMS.Api.StartupExtensions;
using System.Reflection;
using CIS.Infrastructure.Telemetry;

var builder = WebApplication.CreateBuilder(args);

#region register services
// konfigurace aplikace
var appConfiguration = builder.AddFomsConfig();

// vlozit do DI vsechny custom services
builder.Services.AddAttributedServices(typeof(FOMS.Infrastructure.IInfrastructureAssembly), typeof(FOMS.Api.IApiAssembly));

// add CIS pipeline
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisCurrentUser()
    .AddCisWebApiCors()
    .AddCisLogging()
    .AddCisTracing();

// add domain services
builder.Services
    .AddOfferService(true)
    .AddCodebookService(true)
    .AddCustomerService(true)
    .AddProductService(true)
    .AddUserService(true)
    .AddCaseService(true)
    .AddSalesArrangementService(true);

// health checks
builder.AddCisHealthChecks();

// FOMS services
builder
    .AddFomsAuthentication(appConfiguration)
    .AddFomsServices()
    .AddFomsDatabase();
if (appConfiguration.EnableSwaggerUI)
    builder.AddFomsSwagger();

// podpora SPA
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});
#endregion register services

// BUILD APP
var app = builder.Build();

// error middlewares
if (app.Environment.IsProduction())
    app.UseExceptionHandler("/error");
else
    app.UseDeveloperExceptionPage();

// podpora SPA
app
    .UseStaticFiles()
    .UseSpaStaticFiles();

// exception handling
app.UseMiddleware<CIS.Infrastructure.WebApi.Middlewares.ApiExceptionMiddleware>();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseCisWebRequestLocalization();
app.UseCors();
app.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.HttpOptionsMiddleware>();

// autentizace a autorizace
app.UseAuthentication();
//app.UseAuthorization();
app.UseMiddleware<FOMS.Infrastructure.Security.AppSecurityMiddleware>();

// swagger
if (appConfiguration.EnableSwaggerUI)
{
    app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOBY FRONTEND API");
        });
}

// healthchecks
app.MapCisHealthChecks();
// namapovani API modulu
app.AddEndpointsModules(typeof(FOMS.Api.IApiAssembly).GetTypeInfo().Assembly);

// podpora SPA
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.Run();
