using Microsoft.Extensions.DependencyInjection;

namespace CIS.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Zjisteni, zda je dany typ jiz registrovan v DI
    /// </summary>
    public static bool AlreadyRegistered<TService>(this IServiceCollection services)
        => services.Any(t => t.ServiceType == typeof(TService));

    /// <summary>
    /// Abstract method to simplify registration of domain services
    /// </summary>
    public static IServiceCollection AddDomainService<TClient>(this IServiceCollection services)
        where TClient : class
        => throw new NotImplementedException();
}
