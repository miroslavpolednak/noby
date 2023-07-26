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
    var corsConfiguration = builder.Configuration
        .GetSection(CIS.Infrastructure.WebApi.Configuration.CorsConfiguration.AppsettingsConfigurationKey)
        .Get<CIS.Infrastructure.WebApi.Configuration.CorsConfiguration>();

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
            // nevim proc ten posranej .NET middleware pro cors nefunguje... mozna potrebuje autentizaci?
            .Use(async (context, next) => {
                context.Response.OnStarting(() => {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", corsConfiguration!.AllowedOrigins![0]);
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
                    return Task.CompletedTask;
                });

                await next();
            })
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
