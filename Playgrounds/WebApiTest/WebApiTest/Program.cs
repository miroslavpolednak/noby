using CIS.Infrastructure.StartupExtensions;
using DomainServices;
using CIS.InternalServices;

var builder = WebApplication.CreateBuilder(args);

// add CIS pipeline
builder
    .AddCisCoreFeatures()
    .AddCisEnvironmentConfiguration();

// add domain services
builder.Services.AddUserService();
builder.Services.AddHouseholdService();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(WebApiTest.Controllers.TestController).Assembly));

// BUILD APP
var app = builder.Build();

app.UseServiceDiscovery();

// swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();