using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.DomainServicesSecurity.Abstraction;

public static class StartupExtensions
{
    /// <summary>
    /// Pridava moznost ziskani instance fyzickeho uzivatele volajiciho sluzbu
    /// </summary>
    public static IServiceCollection AddCisUserContextHelpers(this IServiceCollection services)
    {
        services.TryAddTransient<ICisUserContextHelpers, CisUserContextHelpers>();
        return services;
    }
}
