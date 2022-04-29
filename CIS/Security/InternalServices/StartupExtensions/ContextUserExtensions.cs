using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Security.InternalServices;

public static class ContextUserExtensions
{
    /// <summary>
    /// Pridava moznost ziskani instance fyzickeho uzivatele volajiciho sluzbu
    /// </summary>
    public static IServiceCollection AddCisContextUser(this IServiceCollection services)
    {
        services.TryAddTransient<ICisUserContextHelpers, CisUserContextHelpers>();

        services.AddScoped<Core.Security.ICurrentUserAccessor, CisCurrentUserAccessor>();
        
        return services;
    }

    /// <summary>
    /// Pridava moznost ziskani instance fyzickeho uzivatele volajiciho sluzbu
    /// </summary>
    public static IApplicationBuilder UseCisContextUser(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CisUserContextMiddleware>();
    }
}
