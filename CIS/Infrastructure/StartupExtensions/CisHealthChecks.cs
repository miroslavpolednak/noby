namespace CIS.Infrastructure.StartupExtensions;

public static class CisHealthChecks
{
    public static WebApplicationBuilder AddCisHealthChecks(this WebApplicationBuilder builder)
    {
        var hc = builder.Services.AddHealthChecks();

        var section = builder.Configuration.GetSection("ConnectionStrings")?.GetChildren();
        if (section != null)
        {
            var elements = section.Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
            foreach (var cs in elements)
                hc.AddSqlServer(cs.Value!, name: cs.Key);
        }

        return builder;
    }

    public static IEndpointConventionBuilder MapCisHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);
    }
}
