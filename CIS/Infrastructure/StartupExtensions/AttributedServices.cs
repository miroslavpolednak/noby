using Microsoft.Extensions.DependencyInjection;
using CIS.Infrastructure.Attributes;
using System;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class AttributedServices
    {
        public static IServiceCollection AddAttributedServices(this IServiceCollection services, params Type[] scannableAssemblies)
        {
            // register all services
            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>().WithoutAttribute<SelfServiceAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime());

            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>().WithAttribute<SelfServiceAttribute>())
                .AsSelf()
                .WithScopedLifetime());
            
            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<TransientServiceAttribute>().WithoutAttribute<SelfServiceAttribute>())
                .AsMatchingInterface()
                .WithTransientLifetime());

            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<TransientServiceAttribute>().WithAttribute<SelfServiceAttribute>())
                .AsSelf()
                .WithTransientLifetime());

            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<SingletonServiceAttribute>().WithoutAttribute<SelfServiceAttribute>())
                .AsMatchingInterface()
                .WithSingletonLifetime());

            services.Scan(selector => selector
                .FromAssembliesOf(scannableAssemblies)
                .AddClasses(x => x.WithAttribute<SingletonServiceAttribute>().WithAttribute<SelfServiceAttribute>())
                .AsSelf()
                .WithSingletonLifetime());

            return services;
        }
    }
}
