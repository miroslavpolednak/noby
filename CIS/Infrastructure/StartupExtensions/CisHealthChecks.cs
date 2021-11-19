using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisHealthChecks
    {
        public static IServiceCollection AddCisHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hc = services.AddHealthChecks();
            var section = configuration.GetSection("ConnectionStrings")?.GetChildren();
            if (section != null)
            {
                foreach (var cs in section)
                    hc.AddSqlServer(cs.Value, name: cs.Key);
            }

            return services;
        }

        public static IEndpointConventionBuilder MapCisHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapHealthChecks("/health");
        }
    }
}
