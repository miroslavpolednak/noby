namespace CIS.Infrastructure.StartupExtensions;

public static class CisHealthChecks
{
    public const string HealthCheckEndpoint = "/health";

    public static WebApplicationBuilder AddCisHealthChecks(this WebApplicationBuilder builder)
    {
        var hc = builder.Services.AddHealthChecks();
        var section = builder.Configuration.GetSection("ConnectionStrings")?.GetChildren();
        if (section != null)
        {
            foreach (var cs in section)
                hc.AddSqlServer(cs.Value, name: cs.Key);
        }

        return builder;
    }

    public static IEndpointConventionBuilder MapCisHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks(HealthCheckEndpoint);
    }

    public static IApplicationBuilder MapCisHealthChecks(this IApplicationBuilder builder)
    {
        builder.UseRouting();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(HealthCheckEndpoint);
        });
        return builder;
    }
}
