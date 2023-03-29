using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.WebApi;
using CIS.Infrastructure.Telemetry;
using NOBY.LogApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddCisEnvironmentConfiguration();
builder
    .AddCisCoreFeatures()
    .AddCisWebApiCors()
    .AddCisLogging();

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
app.UseCisWebApiCors();

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

