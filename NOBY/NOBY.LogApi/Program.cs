using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.WebApi;
using CIS.Infrastructure.Telemetry;
using NOBY.LogApi;
using CIS.Core.Security;

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
    
    builder.Services.AddTransient<ICurrentUserAccessor, CurrentUserAccessor>();

    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseHttpsRedirection();
    app.UseCisWebApiCors();

    if (appConfiguration.EnableSwaggerUi)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
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

