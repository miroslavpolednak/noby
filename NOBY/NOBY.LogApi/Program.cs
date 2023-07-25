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

    var corsConfiguration = builder.Configuration
        .GetSection(CIS.Infrastructure.WebApi.Configuration.CorsConfiguration.AppsettingsConfigurationKey)
        .Get<CIS.Infrastructure.WebApi.Configuration.CorsConfiguration>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "_cors",
            policy =>
            {
                if (corsConfiguration?.AllowedOrigins is not null && corsConfiguration.AllowedOrigins.Any())
                    policy.WithOrigins(corsConfiguration.AllowedOrigins);

                policy
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // pridat swagger
    builder.Services.AddLogApiSwagger();
    
    builder.Services.AddTransient<ICurrentUserAccessor, CurrentUserAccessor>();

    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseHttpsRedirection();
    app.UseCors();

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

