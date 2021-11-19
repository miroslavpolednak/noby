using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Security.InternalServices
{
    public static class ContextUserExtensions
    {
        public static IServiceCollection AddCisContextUser(this IServiceCollection services)
        {
            services.TryAddTransient<ICisUserContextHelpers, CisUserContextHelpers>();
            return services;
        }

        public static IApplicationBuilder UseCisContextUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CisUserContextMiddleware>();
        }
    }
}
