using CIS.Infrastructure.StartupExtensions;
using DomainServices;
using CIS.InternalServices;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// add CIS pipeline
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures();

// add domain services
builder.Services.AddUserService();
builder.Services.AddHouseholdService();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(WebApiTest.Controllers.TestController).Assembly);

// BUILD APP
var app = builder.Build();

app.UseServiceDiscovery();

// swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();