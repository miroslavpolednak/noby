namespace CIS.Infrastructure.StartupExtensions;

public static class CisWebApiCors
{
    public static IServiceCollection AddCisWebApiCors(this IServiceCollection services)
    {
        services.AddCors(x =>
        {
            x.AddPolicy("allowany",
                policyBuilder => policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }
}
