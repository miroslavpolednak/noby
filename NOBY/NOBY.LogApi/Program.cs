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
    builder.Services.AddCisSecurityHeaders();

    // nahrat dokumentaci
    var appConfiguration = new AppConfiguration();
    builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);

    // pridat swagger
    builder.Services.AddLogApiSwagger();
    
    builder.Services.AddTransient<ICurrentUserAccessor, CurrentUserAccessor>();

    var app = builder.Build();
    log.ApplicationBuilt();

    // mapovani endpointu
    app.MapWhen(context => !context.Request.Path.StartsWithSegments("/swagger"), app =>
    {
        app
            .UseRouting()
            .UseCisSecurityHeaders()
            .UseEndpoints(e => e.RegisterLoggerEndpoints());
    });

    if (appConfiguration.EnableSwaggerUi)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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
