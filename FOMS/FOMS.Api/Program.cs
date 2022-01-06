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

Func<HttpContext, bool> isApiCall = (HttpContext context) => context.Request.Path.StartsWithSegments("/api");
Func<HttpContext, bool> isHealthCheck = (HttpContext context) => context.Request.Path.StartsWithSegments(CisHealthChecks.HealthCheckEndpoint);
Func<HttpContext, bool> isSpaCall = (HttpContext context) => !context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/swagger") && !context.Request.Path.StartsWithSegments(CisHealthChecks.HealthCheckEndpoint);

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

// jedna se o SPA call, pust jen tyhle middlewares
app.MapWhen(isSpaCall, appBuilder =>
{
    appBuilder.UseSpaStaticFiles();
    appBuilder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "wwwroot";
    });
});

// health check - neni treba poustet celou pipeline
app.MapWhen(isHealthCheck, appBuilder =>
{
    appBuilder.MapCisHealthChecks();
});

app.UseCisWebRequestLocalization();

//app.UseAuthorization();
app.MapWhen(isApiCall, appBuilder =>
{
    // exception handling
    appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middlewares.ApiExceptionMiddleware>();

    // error middlewares
    if (app.Environment.IsProduction())
        appBuilder.UseExceptionHandler("/error");
    else
        appBuilder.UseDeveloperExceptionPage();

    if (app.Environment.IsProduction())
        appBuilder.UseHsts();

    appBuilder.UseCors();
    appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.HttpOptionsMiddleware>();

    // autentizace a autorizace
    appBuilder.UseAuthentication();
    appBuilder.UseMiddleware<FOMS.Infrastructure.Security.AppSecurityMiddleware>();

    // namapovani API modulu
    appBuilder.AddEndpointsModules(typeof(FOMS.Api.IApiAssembly).GetTypeInfo().Assembly);
});

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

app.Run();