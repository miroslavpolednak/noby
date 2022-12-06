using Microsoft.Extensions.DependencyInjection;

namespace CIS.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Zjisteni, zda je dany typ jiz registrovan v DI
    /// </summary>
    public static bool AlreadyRegistered<TService>(this IServiceCollection services)
        => services.Any(t => t.ServiceType == typeof(TService));
}
