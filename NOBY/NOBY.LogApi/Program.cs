using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.WebApi;
using CIS.Infrastructure.Telemetry;
using NOBY.LogApi;
using CIS.Core.Security;
using NOBY.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

var log = builder.CreateStartupLogger();

try
{
    // konfigurace aplikace
    var appConfiguration = builder.AddNobyConfig();

    builder.AddCisEnvironmentConfiguration();
    builder
        .AddCisCoreFeatures()
        .AddCisWebApiCors()
        .AddCisLogging();
    builder.Services.AddCisSecurityHeaders();

    builder.Services.AddTransient<ICurrentUserAccessor, CurrentUserAccessor>();

    // authentication
    builder.AddNobyAuthentication(appConfiguration);
    builder.Services.AddAuthorization();

    // pridat swagger
    if (appConfiguration.EnableSwaggerUi)
        builder.Services.AddLogApiSwagger();

    var app = builder.Build();
    log.ApplicationBuilt();

    // mapovani endpointu
    app.MapWhen(context => !context.Request.Path.StartsWithSegments("/swagger"), app =>
    {
        // detailed error page
        if (appConfiguration.UseDeveloperExceptionPage)
        {
            app.UseDeveloperExceptionPage();
        }

        app
            .UseCisSecurityHeaders()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            // nevim proc ten posranej .NET middleware pro cors nefunguje... mozna potrebuje autentizaci?
            /*.Use(async (context, next) => {
                context.Response.OnStarting(() => {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", corsConfiguration!.AllowedOrigins![0]);
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
                    return Task.CompletedTask;
                });

                await next();
            })*/
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
