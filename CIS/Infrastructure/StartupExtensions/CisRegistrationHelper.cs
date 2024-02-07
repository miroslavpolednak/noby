using System.Reflection;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisRegistrationHelper
{
    public static IServiceCollection AddAllImlementationOf(this IServiceCollection services, Type type, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        var types = assembly.GetTypes()
             .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

        foreach (var implementationType in types)
        {
            ServiceDescriptor serviceDescriptor = new(type, implementationType, serviceLifetime);
            services.Add(serviceDescriptor);

        }
        return services;
    }
}
