using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using NOBY.LogApi;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures()
    .AddCisWebApiCors()
    .AddCisLogging()
    .AddCisHealthChecks();

// nahrat dokumentaci
var appConfiguration = new AppConfiguration();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);

// pridat swagger
builder.Services.AddLogApiSwagger();

var app = builder.Build();

if (appConfiguration.EnableSwaggerUi)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// mapovani endpointu
app.RegisterLoggerEndpoints();

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}

