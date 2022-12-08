namespace CIS.Infrastructure.StartupExtensions;

public static class CisWebApiCors
{
    private static bool _useCors;
    
    /// <param name="builder"></param>
    /// <returns>True if CORS is enabled an</returns>
    public static WebApplicationBuilder AddCisWebApiCors(this WebApplicationBuilder builder)
    {
        var cisConfiguration = new WebApi.Configuration.CorsConfiguration();
        builder.Configuration.GetSection(WebApi.Configuration.CorsConfiguration.AppsettingsConfigurationKey).Bind(cisConfiguration);

        if (cisConfiguration.EnableCors)
        {
            builder.Services.AddCors(x =>
            {
                x.AddDefaultPolicy(policyBuilder =>
                    {
                        if (cisConfiguration.AllowedOrigins is not null && cisConfiguration.AllowedOrigins.Any())
                            policyBuilder.WithOrigins(cisConfiguration.AllowedOrigins);
                        
                        policyBuilder
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });
            _useCors = true;
        }
        
        return builder;
    }

    public static IApplicationBuilder UseCisWebApiCors(this IApplicationBuilder builder)
    {
        if (_useCors)
            return builder.UseCors();
        else
            return builder;
    }
}
