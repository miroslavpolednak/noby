namespace CIS.Infrastructure.StartupExtensions;

public static class CisHealthChecks
{
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
        return endpoints.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);
    }

    public static IApplicationBuilder MapCisHealthChecks(this IApplicationBuilder builder)
    {
        builder.UseRouting();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);
        });
        return builder;
    }
}
