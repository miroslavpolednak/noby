using CIS.Core.Attributes;

namespace CIS.Infrastructure.StartupExtensions;

public static class AttributedServices
{
    public static IServiceCollection AddAttributedServices(this IServiceCollection services, params Type[] scannableAssemblies)
    {
        // register all services
        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>().WithAttribute<AsImplementedInterfacesServiceAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>().WithAttribute<SelfServiceAttribute>())
            .AsSelf()
            .WithScopedLifetime());
        
        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<TransientServiceAttribute>().WithAttribute<AsImplementedInterfacesServiceAttribute>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<TransientServiceAttribute>().WithAttribute<SelfServiceAttribute>())
            .AsSelf()
            .WithTransientLifetime());

        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<SingletonServiceAttribute>().WithAttribute<AsImplementedInterfacesServiceAttribute>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(selector => selector
            .FromAssembliesOf(scannableAssemblies)
            .AddClasses(x => x.WithAttribute<SingletonServiceAttribute>().WithAttribute<SelfServiceAttribute>())
            .AsSelf()
            .WithSingletonLifetime());

        return services;
    }
}
