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

var builder = WebApplication.CreateBuilder(args);

#region register builder.Services
builder.Services.AddAttributedServices(typeof(FOMS.Infrastructure.IInfrastructureAssembly), typeof(FOMS.Api.IApiAssembly));

// add CIS pipeline
builder.Services.AddCisEnvironmentConfiguration(builder.Configuration);
builder.Services.AddCisCoreFeatures();
builder.Services.AddCisCurrentUser();
builder.Services.AddCisWebApiCors();
builder.Host.AddCisLogging();

// add domain services
builder.Services.AddOfferService(true);
builder.Services.AddCodebookService(true);
builder.Services.AddCustomerService(true);
builder.Services.AddProductService(true);
builder.Services.AddUserService(true);
builder.Services.AddCaseService(true);
builder.Services.AddSalesArrangementService(true);

// health checks
builder.Services.AddCisHealthChecks(builder.Configuration);

// FOMS services
builder.AddFomsConfig();
builder.AddFomsServices();
builder.AddFomsDatabase();
builder.Services.AddFomsSwagger();
#endregion register builder.Services

// BUILD APP
var app = builder.Build();

// error middlewares
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
}
else
{
    app.UseDeveloperExceptionPage();
}

// exception handling
app.UseMiddleware<CIS.Infrastructure.WebApi.Middlewares.ApiExceptionMiddleware>();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseCisWebRequestLocalization();
app.UseCors();
app.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.HttpOptionsMiddleware>();

//app.UseAuthentication();
//app.UseAuthorization();   // HttpContext.User Jak� pr�v� m�?  VS  HttpContext.Endpoint -> jak� pr�va mus� m�t

app.MapCisHealthChecks();
app.AddEndpointsModules(typeof(FOMS.Api.IApiAssembly).GetTypeInfo().Assembly);

app.UseFomsSwagger();

app.Run();
