using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.WebApi;
using CIS.Infrastructure.Telemetry;
using NOBY.LogApi;

var builder = WebApplication.CreateBuilder(args);

var log = builder.CreateStartupLogger();

try
{
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
    log.ApplicationBuilt();

    if (appConfiguration.EnableSwaggerUi)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCisWebApiCors();

    // mapovani endpointu
    app.RegisterLoggerEndpoints();

    log.ApplicationRun();
    app.Run();
}
catch (Exception ex)
{
    log.CatchedException(ex);
}
finally
{
    LoggingExtensions.CloseAndFlush();
}

