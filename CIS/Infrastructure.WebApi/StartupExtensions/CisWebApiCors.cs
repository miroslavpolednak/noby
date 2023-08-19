namespace CIS.Infrastructure.WebApi;

public static class CisWebApiCors
{
    private static bool _useCors;
    public const string NobyCorsPolicyName = "__nobyCorsPolicy";
    
    /// <param name="builder"></param>
    /// <returns>True if CORS is enabled an</returns>
    public static WebApplicationBuilder AddCisWebApiCors(this WebApplicationBuilder builder)
    {
        var cisConfiguration = builder.Configuration
            .GetSection(Configuration.CorsConfiguration.AppsettingsConfigurationKey)
            .Get<Configuration.CorsConfiguration>()!;

        if (cisConfiguration?.EnableCors ?? false)
        {
            builder.Services.AddCors(x =>
            {
                x.AddPolicy(NobyCorsPolicyName, policyBuilder =>
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
            return builder.UseCors(NobyCorsPolicyName);
        else
            return builder;
    }
}
